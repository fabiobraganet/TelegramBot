
namespace ESB.Domain.Entities.Bots
{
    using System;
    using System.Collections.Generic;

    public class MessageIn : BaseEntity
    {
        public Guid MessageInId { get; set; }
        public string AccountId { get; set; }
        public string Messenger { get; set; }
        public string Order { get; set; }
        public string Text { get; set; }
        public DateTime Moment { get; set; }

        public ICollection<MessageOut> MessagesOut { get; set; }
    }
}
