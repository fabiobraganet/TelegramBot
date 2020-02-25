
namespace TelegramBot.CrossCutting.Models.Telegram
{
    public class BotConfiguration
    {
        public string BotToken { get; set; }

        public string Socks5Host { get; set; } = string.Empty;

        public int Socks5Port { get; set; }
    }
}
