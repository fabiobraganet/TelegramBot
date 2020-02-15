
namespace TelegramBot.Application.Services
{
    using Microsoft.Extensions.Logging;
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

        public UpdateService(IBotService botService, ILogger<UpdateService> logger)
        {
            _botService = botService;
            _logger = logger;
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
                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, message.Text);
                    break;

                case MessageType.Photo:
                    var fileId = message.Photo.LastOrDefault()?.FileId;
                    var file = await _botService.Client.GetFileAsync(fileId);

                    var filename = file.FileId + "." + file.FilePath.Split('.').Last();
                    using (var saveImageStream = System.IO.File.Open(filename, FileMode.Create))
                    {
                        await _botService.Client.DownloadFileAsync(file.FilePath, saveImageStream);
                    }

                    await _botService.Client.SendTextMessageAsync(message.Chat.Id, "Thx for the Pics");
                    break;
            }
        }
    }
}
