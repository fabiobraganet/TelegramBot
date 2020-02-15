
namespace TelegramBot.CrossCutting.Interfaces.Telegram
{
    using global::Telegram.Bot.Types;
    using System.Threading.Tasks;

    public interface IUpdateService
    {
        Task EchoAsync(Update update);
    }
}
