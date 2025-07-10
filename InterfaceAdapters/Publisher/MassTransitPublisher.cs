using Application.IPublishers;
using Domain.Interfaces;
using MassTransit;
using Domain.Messages;
using Domain.Contracts;

namespace InterfaceAdapters.Publishers
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
            var eventMessage = new CollaboratorCreatedMessage(
                collaborator.Id,
                collaborator.UserId,
                collaborator.PeriodDateTime
            );

            await _publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator)
        {
            var eventMessage = new CollaboratorUpdatedMessage(
                collaborator.Id,
                collaborator.UserId,
                collaborator.PeriodDateTime
            );

            await _publishEndpoint.Publish(eventMessage);
        }

        public async Task PublishCollaboratorCreationRequestedAsync(CreateCollaboratorCommand message)
        {
            await _publishEndpoint.Publish(message);
        }
    }
}