
namespace ESB.Domain.Entities.Bots
{
    using System;
    
    public class BotMessageOut
    {
        public string MessageId { get; set; }
        public string BotUserId { get; set; }
        public string Text { get; set; }  
        public long SendDate { get; set; }
    }
}
