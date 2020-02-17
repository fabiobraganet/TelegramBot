
namespace ESB.Workers.Bots.Services
{
    using ESB.Domain.Entities.Bots;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Rebus.Activation;
    using Rebus.Bus;
    using Rebus.Config;
    using Rebus.Handlers;
    using System;
    using System.Threading.Tasks;

    public class MessageBotService
    {
        private readonly BuiltinHandlerActivator _activator;
        private readonly ILogger<MessageBotService> _logger;
        private readonly IConfiguration _configuration;
        private IBus _bus;

        public MessageBotService(ILogger<MessageBotService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _activator = new BuiltinHandlerActivator();
        }

        private void ConfigureActivator(
                    BuiltinHandlerActivator activator,
                    string connectionString,
                    string queueName,
                    int workers = 1,
                    int parallel = 1)
        {
            var messageReceived = new MessageReceived<BotMessage>();

            messageReceived.Received += MessageReceived_Received; ;

            activator.Register(() => messageReceived);

            _bus = Configure.With(activator)
                .Transport(t => t.UseRabbitMq(connectionString, queueName)
                //.ExchangeNames(
                //                directExchangeName: "",
                //                topicExchangeName: "")
                )
                .Options(conf =>
                {
                    conf.SetNumberOfWorkers(workers);
                    conf.SetMaxParallelism(parallel);
                })
                .Start();
            
            activator.Bus.Subscribe<BotMessage>().Wait();
            
            Console.Read();
        }

        public void Process()
        {
            string connectionRabbitMQ = _configuration.GetConnectionString("ConexaoRabbitMQ");
            string queueName = typeof(BotMessage).Name;

            ConfigureActivator(_activator, connectionRabbitMQ, queueName, 2, 2);
        }

        private void MessageReceived_Received(object sender, MessageEventArgs<BotMessage> e)
        {
            OnReceived(new MessageEventArgs<BotMessage>(e.Args));
        }

        protected virtual void OnReceived(MessageEventArgs<BotMessage> e)
            => Received?.Invoke(this, e);

        public event EventHandler<MessageEventArgs<BotMessage>> Received;
    }

    
    public class MessageReceived<T>
        : IHandleMessages<T>
    {
        public async Task Handle(T t)
        {
            await OnReceived(new MessageEventArgs<T>(t));
        }

        protected virtual async Task OnReceived(MessageEventArgs<T> e)
            => await Task.Run(() => Received?.Invoke(this, e));

        public event EventHandler<MessageEventArgs<T>> Received;
    }

    public class MessageEventArgs<T>
        : EventArgs
    {
        public T Args { get; set; }

        public MessageEventArgs(T t)
        {
            Args = t;
        }
    }
}
