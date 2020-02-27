
namespace TelegramBot.CrossCutting.Interfaces.Telegram
{
    using global::Telegram.Bot.Types;
    using System.Threading.Tasks;

    public interface IUpdateService
    {
        Task ReceiveMessagesAsync(Update update);
        Task WaitForReturnAsync(Update update);
        Task SendTextMessageAsync(long chatid, string message);
    }
}
