using Application.IPublishers;
using Domain.Interfaces;
using MassTransit;
using Domain.Messages;

namespace WebApi.Publishers
{
    public class MassTransitPublisher : IMessagePublisher
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly ISendEndpointProvider _sendEndpoint;

        public MassTransitPublisher(IPublishEndpoint publishEndpoint, ISendEndpointProvider sendEndpoint)
        {
            _publishEndpoint = publishEndpoint;
            _sendEndpoint = sendEndpoint;
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

        public async Task SendCollaboratorWithoutUserAsync(ICollaboratorWithoutUser collaborator)
        {
            var eventMessage = new CollaboratorWithoutUserCreatedMessage(
                collaborator.Id,
                collaborator.Names,
                collaborator.Surnames,
                collaborator.Email,
                collaborator.DeactivationDate
            );
            var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:collaborators-cmd-{InstanceInfo.InstanceId}"));

            await endpoint.Send(eventMessage);
        }
    }
}