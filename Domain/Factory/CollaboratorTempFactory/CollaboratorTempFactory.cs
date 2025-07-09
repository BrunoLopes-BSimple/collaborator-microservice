using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory.CollaboratorTempFactory;
public class CollaboratorTempFactory : ICollaboratorTempFactory
{
    public ICollaboratorTemp Create(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        Guid collabId = Guid.NewGuid();

        if (periodDateTime._initDate < DateTime.UtcNow || periodDateTime._finalDate > finalDate)
            throw new ArgumentException("Contract dates are invalid!");

        return new CollaboratorTemp(collabId, periodDateTime, names, surnames, email, finalDate);
    }

    public ICollaboratorTemp Create(ICollaboratorTempVisitor visitor)
    {
        return new CollaboratorTemp(visitor.Id, visitor.PeriodDateTime, visitor.Names, visitor.Surnames, visitor.Email, visitor.FinalDate);
    }
}
