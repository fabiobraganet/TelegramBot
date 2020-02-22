
namespace ESB.Domain.Events
{
    using System;

    public class MessageEventArgs<T>
        : EventArgs
    {
        public T Args { get; set; }

        public MessageEventArgs(T t)
        {
            Args = t;
        }
    }
}
