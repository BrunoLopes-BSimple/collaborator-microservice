using Domain.Models;
using Domain.Visitor;

namespace Infrastructure.DataModel;

public class CollaboratorWithoutUserDataModel : ICollaboratorWithoutUserVisitor
{
    public Guid Id { get; set; }
    public string Names { get; set; }
    public string Surnames { get; set; }
    public string Email { get; set; }
    public DateTime? DeactivationDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public CollaboratorWithoutUserDataModel(Guid id, string names, string surnames, string email, DateTime? deactivationDate, PeriodDateTime periodDateTime)
    {
        Id = id;
        Names = names;
        Surnames = surnames;
        Email = email;
        DeactivationDate = deactivationDate;
        PeriodDateTime = periodDateTime;
    }

    public CollaboratorWithoutUserDataModel() { }
}
