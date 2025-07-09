using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public interface IUserFactory
{
    public Task<User?> Create(Guid id);
    public User Create(IUserVisitor userVisitor);
}