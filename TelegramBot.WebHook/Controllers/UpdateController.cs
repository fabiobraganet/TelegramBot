
namespace TelegramBot.WebHook.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Caching.Distributed;
    using System;
    using System.Threading.Tasks;
    using Telegram.Bot.Types;
    using TelegramBot.CrossCutting.Interfaces.Telegram;

    [Route("api/[controller]")]
    public class UpdateController : Controller
    {
        private readonly IUpdateService _updateService;
        private readonly IDistributedCache _cache;

        private readonly DistributedCacheEntryOptions _cacheSetting;

        public UpdateController(IUpdateService updateService, IDistributedCache cache)
        {
            _updateService = updateService;
            _cache = cache;
            _cacheSetting = new DistributedCacheEntryOptions();
            _cacheSetting.SetAbsoluteExpiration(TimeSpan.FromMinutes(2));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            const string TRAFFICCONTROL = "trafficControl";
            
            string cacheKey = $"{TRAFFICCONTROL}_{update.Message.Chat.Id}";

            var trafficControl = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrWhiteSpace(trafficControl))
            {
                await _cache.SetStringAsync(key: cacheKey, value: "1");

                await _updateService.EchoAsync(update);

                await _cache.RemoveAsync(key: cacheKey);
            }
            else
            {
                await _updateService.WaitForReturnAsync(update);
            }

            return Ok();
        }
    }
}
