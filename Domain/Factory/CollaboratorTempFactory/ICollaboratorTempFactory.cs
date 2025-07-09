using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory.CollaboratorTempFactory;
public interface ICollaboratorTempFactory
{
    ICollaboratorTemp Create(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate);

    ICollaboratorTemp Create(ICollaboratorTempVisitor visitor);
}
