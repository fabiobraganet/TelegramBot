
namespace ESB.Domain.Entities.Bots
{
    using Services;
    using System.Threading;

    public class BotOperator
    {
        public IMessaging<BotMessage> Messaging { get; set; }
        public string ConnectionId { get; set; }
        public CancellationTokenSource CancellationTokenSource { get; set; }
    }
}
