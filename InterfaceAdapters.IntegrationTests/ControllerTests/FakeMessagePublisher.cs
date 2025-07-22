using Application.IPublishers;
using Domain.Interfaces;

namespace InterfaceAdapters.IntegrationTests.ControllerTests;

public class FakeMessagePublisher : IMessagePublisher
{
    public Task PublishCollaboratorCreatedAsync(ICollaborator collaborator)
    {
        return Task.CompletedTask;
    }

    public Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator)
    {
        return Task.CompletedTask;
    }
}
