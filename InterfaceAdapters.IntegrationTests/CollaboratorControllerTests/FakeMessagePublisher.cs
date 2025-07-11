using Application.IPublishers;
using Domain.Interfaces;

public class FakeMessagePublisher : IMessagePublisher
{
    public Task PublishCollaboratorCreatedAsync(ICollaborator collaborator)
    {
        // Simulate success without doing anything
        return Task.CompletedTask;
    }

    public Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator)
    {
        // Simulate success without doing anything
        return Task.CompletedTask;
    }
}
