
namespace TelegramBot.Application.Services
{
    using ESB.Domain.Entities.Bots;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Rebus.Activation;
    using Rebus.Bus;
    using Rebus.Config;
    using Rebus.Transport;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using Telegram.Bot.Types.Enums;
    using TelegramBot.CrossCutting.Interfaces.Telegram;

    public class UpdateService : IUpdateService
    {
        private readonly IBotService _botService;
        private readonly ILogger<UpdateService> _logger;

        private readonly BuiltinHandlerActivator _activator;

        public UpdateService(IBotService botService, ILogger<UpdateService> logger, IConfiguration configuration)
        {
            _activator = new BuiltinHandlerActivator();
            _botService = botService;
            _logger = logger;

            string connectionRabbitMQ = configuration.GetConnectionString("ConexaoRabbitMQ");
            string queueName = typeof(BotMessage).Name;

            Configure
                .With(_activator)
                .Transport(t => t.UseRabbitMq(connectionRabbitMQ, queueName)
                                    .EnablePublisherConfirms(value: true))
                .Create();
        }

        private async Task PublishMessage(Message message)
        {
            var esbmessage = new BotMessage()
            {
                MessageId = message.MessageId.ToString(),
                BotUserId = message.Chat.Id.ToString(),
                Text = message.Text,
                SendDate = message.Date.Ticks
            };

            using (var scope = new RebusTransactionScope())
            {
                await _activator.Bus.SendLocal(esbmessage).ConfigureAwait(false);
                await scope.CompleteAsync().ConfigureAwait(false);
            }
        }

        public async Task EchoAsync(Update update)
        {
            if (update.Type != UpdateType.Message)
            {
                if (update.Type == UpdateType.EditedMessage)
                {
                    var edited = update.EditedMessage;
                    
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("O sistema não aceita edições de mensagens.");
                    sb.AppendLine("Por favor, não edite mensagens.");

                    await _botService.Client.SendTextMessageAsync(edited.Chat.Id, sb.ToString());
                }

                return;
            }

            var message = update.Message;

            _logger.LogInformation($"{DateTime.UtcNow} - TELEGRAM USERID {message.Chat.Id}");

            switch (message.Type)
            {
                case MessageType.Text:

                    await PublishMessage(message).ConfigureAwait(false);
                    
                    break;

                case MessageType.Photo:
                    var fileId = message.Photo.LastOrDefault()?.FileId;
                    var file = await _botService.Client.GetFileAsync(fileId);

                    var filename = file.FileId + "." + file.FilePath.Split('.').Last();
                    using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                    {
                        await _botService.Client.DownloadFileAsync(file.FilePath, saveImageStream);
                    }

                    await SendTextMessageAsync(message.Chat.Id, "Imagem recebida!");
                    break;
            }
        }

        public async Task WaitForReturnAsync(Update update)
        {
            var message = update.Message;
            
            await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Ainda estamos processando a mensagem anterior. Por favor, aguarde.");
        }

        public async Task SendTextMessageAsync(long chatid, string message)
        {
            await _botService.Client.SendTextMessageAsync(chatId: chatid, text: message);
        }
    }
}
