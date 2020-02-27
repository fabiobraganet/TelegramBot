
namespace TelegramBot.WebHook
{
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public class Program
    {
        private static ILogger<Program> _logger;

        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();
            
            _logger = host.Services.GetRequiredService<ILogger<Program>>();

            _logger.LogInformation("Iniciando o WebHook dos bots");
            
            host.Run();

            _logger.LogInformation("O WebHook dos bots foi finalizado");
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost
            .CreateDefaultBuilder(args)
            .UseStartup<Startup>();
    }
}
