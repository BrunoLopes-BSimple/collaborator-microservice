using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;
namespace Infrastructure.DataModel;
public class CollaboratorDataModel : ICollaboratorVisitor
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public CollaboratorDataModel(ICollaborator collaborator)
    {
        Id = collaborator.Id;
        UserId = collaborator.UserId;
        PeriodDateTime = collaborator.PeriodDateTime;
    }

    public CollaboratorDataModel()
    {
    }

    public CollaboratorDataModel(Guid id, Guid userId, PeriodDateTime periodDateTime)
    {
        Id = id;
        UserId = userId;
        PeriodDateTime = periodDateTime;
    }
}
