using Application.ISender;
using Domain.Contracts;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class FakeMessageSender : IMessageSender
{
    public Task SendCollaboratorCreationCommandAsync(CreateCollaboratorCommand message)
    {
        return Task.CompletedTask;
    }

    public Task SendUserForCollabCommandAsync(UserForCollabCommandMessage command)
    {
        return Task.CompletedTask;
    }
}
