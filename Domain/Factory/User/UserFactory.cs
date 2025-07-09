using Domain.IRepository;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public class UserFactory : IUserFactory
{

    private readonly IUserRepository _userRepository;

    public UserFactory(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Create(Guid id)
    {
        var alreadyExists = await _userRepository.Exists(id);

        if (alreadyExists)
        {
            return null;
        }

        return new User(id);
    }

    public User Create(IUserVisitor userVisitor)
    {
        return new User(userVisitor.Id);
    }
}