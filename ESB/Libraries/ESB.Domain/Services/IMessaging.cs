
namespace ESB.Domain.Services
{
    using Events;
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public interface IMessaging<T>
    {
        Task Execute(CancellationToken cancellationToken);

        event EventHandler<MessageEventArgs<T>> Received;

        bool IsConnected { get; set; }
    }
}
