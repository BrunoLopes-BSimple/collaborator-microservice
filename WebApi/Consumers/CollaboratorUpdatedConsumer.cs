using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Messaging;
using MassTransit;

namespace WebApi.Consumers
{
    public class CollaboratorUpdatedConsumer : ConsumerDefinition<CollaboratorUpdatedConsumer>, IConsumer<CollaboratorUpdatedEvent>
    {
        private readonly ICollaboratorService _collabService;

        public CollaboratorUpdatedConsumer(ICollaboratorService collabService)
        {
            _collabService = collabService;
            EndpointName = "cmd-collaborator-updated-events";
        }

        public async Task Consume(ConsumeContext<CollaboratorUpdatedEvent> context)
        {
            await _collabService.UpdateCollaboratorReferenceAsync(context.Message.Id,context.Message.UserId, context.Message.PeriodDateTime);
        }
    }
}