using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.CrossCutting.Models.Telegram
{
    public class BotConfiguration
    {
        public string BotToken { get; set; } = "823321556:AAFH3TZsLJAYyas4XiyjyEkoqdGFZvTXxU4";

        public string Socks5Host { get; set; } = string.Empty;

        public int Socks5Port { get; set; }
    }
}
