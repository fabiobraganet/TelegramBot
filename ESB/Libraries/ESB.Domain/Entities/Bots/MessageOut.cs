
namespace ESB.Domain.Entities.Bots
{
    using System;
    
    public class MessageOut : BaseEntity
    {
        public Guid MessageOutId { get; set; }
        public string AccountId { get; set; }
        public string Messenger { get; set; }
        public string Text { get; set; }

        public virtual MessageIn MessageIn { get; set; }
    }
}
