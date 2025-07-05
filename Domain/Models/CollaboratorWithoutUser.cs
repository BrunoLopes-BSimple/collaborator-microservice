using Domain.Interfaces;

namespace Domain.Models;

public class CollaboratorWithoutUser : ICollaboratorWithoutUser
{
    public Guid Id { get; }
    public string Names { get; private set; }
    public string Surnames { get; private set; }
    public string Email { get; private set; }
    public DateTime DeactivationDate { get; private set; }
    public PeriodDateTime PeriodDateTime { get; private set; }

    public CollaboratorWithoutUser(string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime)
    {
        Id = Guid.NewGuid();
        Names = names;
        Surnames = surnames;
        Email = email;
        DeactivationDate = deactivationDate;
        PeriodDateTime = periodDateTime;
    }

    public CollaboratorWithoutUser(Guid id, string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime)
    {
        Id = id;
        Names = names;
        Surnames = surnames;
        Email = email;
        DeactivationDate = deactivationDate;
        PeriodDateTime = periodDateTime;
    }
}
