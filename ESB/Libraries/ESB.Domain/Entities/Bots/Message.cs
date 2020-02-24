
namespace ESB.Domain.Entities.Bots
{
    using System;
    
    public class Message : BaseEntity
    {
        public string AccountId { get; set; }
        public string Messenger { get; set; } = "Telegram";
        public string Order { get; set; }
        public string Text { get; set; }
        public DateTime Moment { get; set; }
    }
}
