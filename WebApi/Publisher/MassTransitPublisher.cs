using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Messaging;
using Domain.Interfaces;
using MassTransit;

namespace WebApi.Publishers
{
    public class MassTransitPublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;

        public MassTransitPublisher(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;
        }

        public async Task PublishCollaboratorCreatedAsync(ICollaborator collaborator)
        {
            var eventMessage = new CollaboratorCreatedEvent(
                collaborator.Id,
                collaborator.UserId,
                collaborator.PeriodDateTime
            );

            await _publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator)
        {
            var eventMessage = new CollaboratorUpdatedEvent(
                collaborator.Id,
                collaborator.UserId,
                collaborator.PeriodDateTime
            );

            await _publishEndpoint.Publish(eventMessage);
        }
    }
}