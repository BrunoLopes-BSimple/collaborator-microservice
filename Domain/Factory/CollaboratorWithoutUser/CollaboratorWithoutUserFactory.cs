using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public class CollaboratorWithoutUserFactory : ICollaboratorWithoutUserFactory
{
    public CollaboratorWithoutUserFactory()
    {
    }

    public CollaboratorWithoutUser Create(string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime)
    {
        return new CollaboratorWithoutUser(names, surnames, email, deactivationDate, periodDateTime);
    }

    public CollaboratorWithoutUser Create(ICollaboratorWithoutUserVisitor visitor)
    {
        return new CollaboratorWithoutUser(visitor.Id, visitor.Names, visitor.Surnames, visitor.Email, visitor.DeactivationDate, visitor.PeriodDateTime);
    }
}
