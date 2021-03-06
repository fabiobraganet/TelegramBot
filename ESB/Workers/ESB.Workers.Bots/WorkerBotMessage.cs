
namespace ESB.Workers.Bots
{
    using ESB.Data.Messaging;
    using ESB.Domain.Entities.Bots;
    using Grpc.Net.Client;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Rebus.Activation;
    using Rebus.Config;
    using Rebus.Transport;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class WorkerBotMessage : BackgroundService
    {
        private readonly ILogger<WorkerBotMessage> _logger;
        private readonly IConfiguration _configuration;        

        public WorkerBotMessage(ILogger<WorkerBotMessage> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionRabbitMQ = _configuration.GetConnectionString("ConexaoRabbitMQ");
            var queueName = typeof(BotMessage).Name;
            var mensageria = new MessageOperator<BotMessage>(connectionRabbitMQ, queueName, "a", "b", 1, 1);

            try
            {
                mensageria.Received += Mensageria_Received;
                await mensageria.Execute(stoppingToken).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            finally
            {
                _logger.LogInformation("Finalizando a execu��o do WorkerBotMessage");
            }
        }

        private void Mensageria_Received(object sender, Domain.Events.MessageEventArgs<BotMessage> e)
        {
            var s = JsonConvert.SerializeObject(e.Args);
            _logger.LogInformation(s);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

            var uri = _configuration.GetConnectionString("GRPCServicesMessaging");
            var msg = e.Args;
            var message = new ESB.Services.Messaging.MessageIn()
            {
                MessageId = msg.MessageId,
                BotUserId = msg.BotUserId,
                Text = msg.Text,
                SendDate = msg.SendDate.ToString()
            };

            using var channel = GrpcChannel.ForAddress(uri);
            var client = new ESB.Services.Messaging.Messages.MessagesClient(channel);
            var reply = client.ProcessMessage(message);

            var _activator = new BuiltinHandlerActivator();
            var connectionRabbitMQ = _configuration.GetConnectionString("ConexaoRabbitMQ");
            var queueName = typeof(BotMessageOut).Name;

            Configure
                .With(_activator)
                .Transport(t => t.UseRabbitMq(connectionRabbitMQ, queueName)
                                    .EnablePublisherConfirms(value: true))
                .Create();

            var esbmessage = new BotMessageOut()
            {
                MessageId = msg.MessageId.ToString(),
                BotUserId = msg.BotUserId.ToString(),
                Text = reply.Message,
                SendDate = DateTime.UtcNow.Ticks
            };

            using (var scope = new RebusTransactionScope())
            {
                _activator.Bus.SendLocal(esbmessage).Wait();
                scope.CompleteAsync().Wait();
            }

        }

    }
}
