
namespace ESB.Workers.Bots
{
    using ESB.Workers.Bots.Services;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class WorkerBotMessage : BackgroundService
    {
        private readonly ILogger<WorkerBotMessage> _logger;
        private readonly MessageBotService _messageBotService; 

        public WorkerBotMessage(ILogger<WorkerBotMessage> logger, MessageBotService messagebotservice)
        {
            _logger = logger;
            _messageBotService = messagebotservice;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Thread execute = new Thread(ConsumerMessageBots) { Priority = ThreadPriority.Highest };
            try
            {
                execute.Start();

                while (!stoppingToken.IsCancellationRequested)
                    await Task.Delay(1000, stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                execute.Abort();
            }
            finally
            {
                if (execute.IsAlive) 
                    execute.Abort();

                _logger.LogInformation("Finalizando a execução do WorkerBotMessage");
            }
        }

        private void ConsumerMessageBots()
        {
            _messageBotService.Process();
        }
    }
}
