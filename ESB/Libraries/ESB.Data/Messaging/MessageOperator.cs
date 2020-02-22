
namespace ESB.Data.Messaging
{
    using Domain.Events;
    using Domain.Services;

    using Rebus.Activation;
    using Rebus.Config;
    using Rebus.Logging;

    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public class MessageOperator<T> : IMessaging<T>
        where T : class, new()
    {
        private readonly string _connectionString;
        private readonly string _queueName;
        private readonly string _exchangeDirectTypeName;
        private readonly string _exchangeTopicTypeName;
        private readonly int _queueNumberOfWorkers;
        private readonly int _queueWorkMaxParallelism;

        public MessageOperator(
            string connectionstring,
            string queuename,
            string exchangedirecttypename,
            string exchangetopictypename,
            int queuenumberofworkers,
            int queueworkmaxparallelism)
        {
            if (string.IsNullOrEmpty(connectionstring))
                throw new ArgumentNullException("connectionstring received a null argument!");

            if (string.IsNullOrEmpty(connectionstring))
                throw new ArgumentNullException("queuename received a null argument!");

            if (string.IsNullOrEmpty(exchangedirecttypename))
                throw new ArgumentNullException("exchangedirecttypename received a null argument!");

            if (string.IsNullOrEmpty(exchangetopictypename))
                throw new ArgumentNullException("exchangetopictypename received a null argument!");

            if (queuenumberofworkers == 0)
                throw new ArgumentNullException("queuenumberofworkers received a null argument!");

            if (queueworkmaxparallelism == 0)
                throw new ArgumentNullException("queuename received a null argument!");

            _connectionString = connectionstring;
            _queueName = queuename;

            _exchangeDirectTypeName = exchangedirecttypename;
            _exchangeTopicTypeName = exchangetopictypename;

            _queueNumberOfWorkers = queuenumberofworkers;
            _queueWorkMaxParallelism = queueworkmaxparallelism;
        }

        public bool IsConnected { get; set; }

        public async Task Execute(CancellationToken cancellationToken)
        {
            await Task.Run(() => RaiseConsumer(cancellationToken));
        }

        private async Task RaiseConsumer(CancellationToken cancellationToken)
        {
            try
            {
                using (var adapter = new BuiltinHandlerActivator())
                {
                    var messageReceived = new MessageReceived<T>();

                    messageReceived.Received += MessageReceived_Received;

                    adapter.Register(() => messageReceived);

                    Configure
                        .With(adapter)
                        .Logging(l => l.ColoredConsole(LogLevel.Error))
                        .Transport(t => t.UseRabbitMq(
                            connectionString: _connectionString,
                            inputQueueName: _queueName))
                            //.ExchangeNames(
                            //    directExchangeName: _exchangeDirectTypeName,
                            //    topicExchangeName: _exchangeTopicTypeName))
                        .Options(o =>
                        {
                            o.SetNumberOfWorkers(_queueNumberOfWorkers);
                            o.SetMaxParallelism(_queueWorkMaxParallelism);
                        })
                        .Start();

                    adapter.Bus.Subscribe<T>().Wait();

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(1000);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void MessageReceived_Received(object sender, MessageEventArgs<T> e)
        {
            OnReceived(new MessageEventArgs<T>(e.Args));
        }

        protected virtual void OnReceived(MessageEventArgs<T> e)
            => Received?.Invoke(this, e);

        public event EventHandler<MessageEventArgs<T>> Received;
    }
}
