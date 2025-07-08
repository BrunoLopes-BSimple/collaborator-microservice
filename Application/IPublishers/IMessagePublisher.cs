using Application.DTO.Collaborators;
using Domain.Interfaces;

namespace Application.IPublishers;

public interface IMessagePublisher
{
    Task PublishCollaboratorCreatedAsync(ICollaborator collaborator);
    Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator);
    Task SendCollaboratorWithoutUserCreatedAsync(CreateCollaboratorWithoutUserDTO collaborator);
    Task SendCreateUserFromCollaboratorCommandAsync(CreatedCollaboratorWithoutUserDTO dto, Guid correlationId);
}