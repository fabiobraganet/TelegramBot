# TelegramBot
Projeto de uso e boas práticas usando a API do Telegram

Para usar, inclua o arquivo de configuração de desenvolvimento (appsettings.Development.json) no projeto web.
Use:

{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "System": "Information",
      "Microsoft": "Information"
    }
  },
  "BotConfiguration": {
    "BotToken": "{SEU ID DO BotFather}",
    "Socks5Host": "",
    "Socks5Port": 0
  }
}

Para obter um ID do Telegram use o @BotFather do Telegram
