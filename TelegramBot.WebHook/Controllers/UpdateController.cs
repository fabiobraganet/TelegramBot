
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
            _cacheSetting.SetAbsoluteExpiration(TimeSpan.FromSeconds(30));
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Update update)
        {
            const string TRAFFICCONTROL = "trafficControl";
            
            string cacheKey = $"{TRAFFICCONTROL}_{update.Message.Chat.Id}";

            var trafficControl = await _cache.GetStringAsync(cacheKey);

            if (string.IsNullOrWhiteSpace(trafficControl))
            {
                _cache.SetStringAsync(key: cacheKey, value: "1").Wait();

                _updateService.ReceiveMessagesAsync(update).Wait();

                //await _cache.RemoveAsync(key: cacheKey).ConfigureAwait(false);
            }
            else
            {
                await _updateService.WaitForReturnAsync(update).ConfigureAwait(false);
            }

            return Ok();
        }
    }
}
