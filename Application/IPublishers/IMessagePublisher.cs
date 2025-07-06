using Domain.Interfaces;

namespace Application.IPublishers
{
    public interface IMessagePublisher
    {
        Task PublishCollaboratorCreatedAsync(ICollaborator collaborator);
        Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator);
        Task SendCollaboratorWithoutUserAsync(ICollaboratorWithoutUser collaborator);

    }
}