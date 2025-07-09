using Domain.Interfaces;
using Domain.Models;
using Domain.Visitor;
namespace Infrastructure.DataModel;
public class CollaboratorDataModel : ICollaboratorVisitor
{
    public required Guid Id { get; set; }
    public required Guid UserId { get; set; }
    public required PeriodDateTime PeriodDateTime { get; set; }
    public CollaboratorDataModel()
    {
    }

    public CollaboratorDataModel(ICollaborator collaborator)
    {
        Id = collaborator.Id;
        UserId = collaborator.UserId;
        PeriodDateTime = collaborator.PeriodDateTime;
    }

}
