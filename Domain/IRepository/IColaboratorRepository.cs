using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;

public interface ICollaboratorRepository : IGenericRepositoryEF<ICollaborator, Collaborator, ICollaboratorVisitor>
{
    Task<bool> ExistsByUserIdAsync(Guid userId);
    Task<bool> AlreadyExistsAsync(Guid collbId);
    Task<bool> IsRepeated(ICollaborator collaborator);
    Task<IEnumerable<ICollaborator>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<ICollaborator>> GetByUsersIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<Collaborator>> GetActiveCollaborators();
    Task<long> GetCount();
    Task<Collaborator?> UpdateCollaborator(ICollaborator collab);
}