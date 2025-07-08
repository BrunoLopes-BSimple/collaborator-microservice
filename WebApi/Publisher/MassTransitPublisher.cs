using Application.IPublishers;
using Domain.Interfaces;
using MassTransit;
using Domain.Messages;
using Domain.Models;
using Application.DTO.Collaborators;
using Domain.Commands;

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

        public async Task SendCollaboratorWithoutUserCreatedAsync(CreateCollaboratorWithoutUserDTO collaborator)
        {
            var eventMessage = new CollaboratorWithoutUserStartSagaMessage(
                Guid.NewGuid(),
                collaborator.Names,
                collaborator.Surnames,
                collaborator.Email,
                collaborator.DeactivationDate,
                collaborator.PeriodDateTime
            );

            var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri($"queue:collaborators-cmd-{InstanceInfo.InstanceId}"));
            await endpoint.Send(eventMessage);
        }

        public async Task SendCreateUserFromCollaboratorCommandAsync(CreatedCollaboratorWithoutUserDTO collaborator, Guid correlationId)
        {
            var eventMessage = new CreateUserFromCollaboratorCommand(correlationId,
                InstanceInfo.InstanceId,
                collaborator.Id,
                collaborator.Names,
                collaborator.Surnames,
                collaborator.Email,
                collaborator.DeactivationDate
            );

            var endpoint = await _sendEndpoint.GetSendEndpoint(new Uri("queue:users-cmd-saga"));
            await endpoint.Send(eventMessage);
        }
    }
}