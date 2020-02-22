
namespace ESB.Domain.Events
{
    using Rebus.Handlers;
    using System;
    using System.Threading.Tasks;

    public class MessageReceived<T>
            : IHandleMessages<T>
    {
        public async Task Handle(T t)
        {
            await OnReceived(new MessageEventArgs<T>(t));
        }

        protected virtual async Task OnReceived(MessageEventArgs<T> e)
            => await Task.Run(() => Received?.Invoke(this, e));

        public event EventHandler<MessageEventArgs<T>> Received;
    }
}
