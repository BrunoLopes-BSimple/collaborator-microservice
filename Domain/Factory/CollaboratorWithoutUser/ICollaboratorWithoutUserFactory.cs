using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory;

public interface ICollaboratorWithoutUserFactory
{
    CollaboratorWithoutUser Create(string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime);

    CollaboratorWithoutUser Create(ICollaboratorWithoutUserVisitor visitor);

}

