
namespace ESB.Domain.Entities.Bots
{
    using System;
    
    public class BotMessage
    {
        public string MessageId { get; set; }
        public string BotUserId { get; set; }
        public string Text { get; set; }  
        public DateTime SendDate { get; set; }
    }
}
