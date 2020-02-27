
namespace TelegramBot.Application.Services
{
    using ESB.Data.Messaging;
    using ESB.Domain.Entities.Bots;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using TelegramBot.CrossCutting.Interfaces.Telegram;

    public class WorkerBotMessage : BackgroundService
    {
        private readonly ILogger<WorkerBotMessage> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUpdateService _updateService;
        private readonly IDistributedCache _cache;

        public IServiceScopeFactory _serviceScopeFactory;

        public WorkerBotMessage(IServiceScopeFactory serviceScopeFactory, ILogger<WorkerBotMessage> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceScopeFactory = serviceScopeFactory;

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                _updateService = scope.ServiceProvider.GetRequiredService<IUpdateService>();
                _cache = scope.ServiceProvider.GetRequiredService<IDistributedCache>();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var connectionRabbitMQ = _configuration.GetConnectionString("ConexaoRabbitMQ");
            var queueName = typeof(BotMessageOut).Name;
            var mensageria = new MessageOperator<BotMessageOut>(connectionRabbitMQ, queueName, "c", "d", 1, 1);

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
                _logger.LogInformation("Finalizando a execução do WorkerBotMessage de saída");
            }
        }

        private void Mensageria_Received(object sender, ESB.Domain.Events.MessageEventArgs<BotMessageOut> e)
        {
            const string TRAFFICCONTROL = "trafficControl";

            var s = JsonConvert.SerializeObject(e.Args);
            _logger.LogInformation(s);

            var message = e.Args;

            _updateService
                .SendTextMessageAsync(long.Parse(message.BotUserId), message.Text)
                .Wait();

            var cacheKey = $"{TRAFFICCONTROL}_{message.BotUserId}";

            var trafficControl = _cache.GetStringAsync(cacheKey).GetAwaiter().GetResult();

            if (!string.IsNullOrWhiteSpace(trafficControl))
            {
                _cache.RemoveAsync(key: cacheKey).Wait();
            }
        }
    }
}
