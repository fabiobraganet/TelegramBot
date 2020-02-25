
namespace ESB.Services.Messaging
{
    using System;
    using System.Threading.Tasks;
    using ESB.Domain.Interfaces;
    using Grpc.Core;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    public class BotMessageService : Messages.MessagesBase
    {
        private readonly IRepository<ESB.Domain.Entities.Bots.MessageIn> _messageRep;
        private readonly ILogger<BotMessageService> _logger;
        public BotMessageService(IRepository<ESB.Domain.Entities.Bots.MessageIn> messagerep, ILogger<BotMessageService> logger)
        {
            _messageRep = messagerep;
            _logger = logger;
        }

        public override Task<MessageOut> ProcessMessage(MessageIn request, ServerCallContext context)
        {
            _logger.LogInformation($"Mensagem recebida: {request.BotUserId} {request.MessageId}");

            _messageRep.Insert(new ESB.Domain.Entities.Bots.MessageIn() 
            {
                //Id = Guid.NewGuid(),
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
