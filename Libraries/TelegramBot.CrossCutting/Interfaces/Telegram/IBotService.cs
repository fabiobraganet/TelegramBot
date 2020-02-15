
namespace TelegramBot.CrossCutting.Interfaces.Telegram
{
    using global::Telegram.Bot;

    public interface IBotService
    {
        TelegramBotClient Client { get; }
    }
}
