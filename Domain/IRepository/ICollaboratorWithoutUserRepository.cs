using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.IRepository;

public interface ICollaboratorWithoutUserRepository : IGenericRepositoryEF<ICollaboratorWithoutUser, CollaboratorWithoutUser, ICollaboratorWithoutUserVisitor>
{
    Task<bool> AlreadyExistsAsync(Guid collbId);
    Task<IEnumerable<ICollaboratorWithoutUser>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task<IEnumerable<ICollaboratorWithoutUser>> GetByUsersIdsAsync(IEnumerable<Guid> ids);
    Task<ICollaboratorWithoutUser?> GetByEmailAsync(string email);

}