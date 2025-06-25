using Application.Interfaces;
using Application.Messaging;
using MassTransit;

namespace WebApi.Consumers
{
    public class CollaboratorConsumer : ConsumerDefinition<CollaboratorConsumer>, IConsumer<CollaboratorCreatedEvent>
    {
        private readonly ICollaboratorService _collabService;

        public CollaboratorConsumer(ICollaboratorService collabService)
        {
            _collabService = collabService;
            EndpointName = "cmd-collaborator-created-events";
        }

        public async Task Consume(ConsumeContext<CollaboratorCreatedEvent> context)
        {
            await _collabService.AddCollaboratorReferenceAsync(context.Message.Id, context.Message.UserId, context.Message.PeriodDateTime);
        }
    }
}