using Application.ISenders;
using Domain.Messages;
using Domain.Models;
using MassTransit;

namespace WebApi.Sender;
public class MassTransitSender : IMessageSender
{
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MassTransitSender(ISendEndpointProvider sendEndpointProvider)
    {
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task SendCollaboratorTempCreationCommandAsync(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:collab-user-saga"));
        await endpoint.Send(new CollaboratorTempCreationCommandMessage(periodDateTime, names, surnames, email, finalDate));
    }

    public async Task SendUserForCollabCommandAsync(Guid id, PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        var endpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:user-for-collab"));
        await endpoint.Send(new UserForCollabCommandMessage(id, periodDateTime, names, surnames, email, finalDate));
    }
}