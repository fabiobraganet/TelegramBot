
namespace ESB.Services.Messaging
{
    using System;
    using System.Threading.Tasks;
    using ESB.Domain.Entities.Bots;
    using ESB.Domain.Interfaces;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class BotMessageService : Messages.MessagesBase
    {
        private readonly IRepository<Message> _messageRep;
        private readonly ILogger<BotMessageService> _logger;
        public BotMessageService(IRepository<Message> messagerep, ILogger<BotMessageService> logger)
        {
            _messageRep = messagerep;
            _logger = logger;
        }

        public override Task<MessageOut> ProcessMessage(MessageIn request, ServerCallContext context)
        {
            _logger.LogInformation($"Mensagem recebida: {request.BotUserId} {request.MessageId}");

            _messageRep.Insert(new Message() 
            {
                Id = Guid.NewGuid(),
                Order = request.MessageId,
                AccountId = request.BotUserId,
                Moment = DateTime.Now,
                Text = request.Text
            });

            var body = JsonConvert.SerializeObject(request);

            return Task.FromResult(new MessageOut
            {
                Result = true,
                Message = body
            });
        }

    }
}
