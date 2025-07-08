using Application.DTO;
using Application.DTO.Collaborators;
using Domain.Interfaces;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ICollaboratorService
    {
        Task<ICollaborator?> AddCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);
        Task<Result<CreatedCollaboratorDTO>> Create(CreateCollaboratorDTO collabDto);
        Task<CreatedCollaboratorWithoutUserDTO> CreateWithoutUser(CreateCollaboratorWithoutUserDTO collabDto, Guid correlationId);
        Task<Result<CreateCollaboratorWithoutUserDTO>> StartSagaCollabWithoutUser(CreateCollaboratorWithoutUserDTO collabDto);
        Task<Result<CollabUpdatedDTO>?> EditCollaborator(CollabData dto);
        Task<bool> AddUserIdForCollaboratorAsync(Guid userId, Guid collaboratorId);
        Task<ICollaborator?> UpdateCollaboratorReferenceAsync(Guid collabId, Guid userId, PeriodDateTime period);

    }
}