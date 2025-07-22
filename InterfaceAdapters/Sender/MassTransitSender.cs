using Application.ISender;
using Domain.Contracts;
using Domain.Models;
using MassTransit;

namespace InterfaceAdapters.Sender
{
    public class MassTransitSender : IMessageSender
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public MassTransitSender(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        public async Task SendCollaboratorCreationCommandAsync(CreateCollaboratorCommand message)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:collaborator-creation-saga"));
            await endpoint.Send(message);
        }

        public async Task SendUserForCollabCommandAsync(UserForCollabCommandMessage message)
        {
            var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:user-for-collab"));
            await endpoint.Send(message);
        }
    }
}