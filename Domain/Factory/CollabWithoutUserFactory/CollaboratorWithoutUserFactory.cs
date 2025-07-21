using Domain.Models;
using Domain.Visitor;

namespace Domain.Factory.CollabWithoutUserFactory
{
    public class CollaboratorWithoutUserFactory : ICollaboratorWithoutUserFactory
    {

        public CollaboratorWithoutUserFactory()
        {
        }

        public CollaboratorWithoutUser Create(ICollaboratorWithoutUserVisitor visitor)
        {
            return new CollaboratorWithoutUser(
                visitor.Id,
                visitor.Names,
                visitor.Surnames,
                visitor.Email,
                visitor.DeactivationDate,
                visitor.PeriodDateTime);
        }
    }
}