using Domain.Interfaces;

namespace Application.Interfaces;

public interface IUserService
{
    public Task<IUser?> AddUserReferenceAsync(Guid userId);
}
