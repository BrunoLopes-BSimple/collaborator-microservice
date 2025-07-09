using Domain.Interfaces;

namespace Domain.Models;
public class CollaboratorTemp : ICollaboratorTemp
{
    public Guid Id { get; }
    public PeriodDateTime PeriodDateTime { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; }

    public CollaboratorTemp(Guid id, PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        Id = id;
        PeriodDateTime = periodDateTime;
        Names = names;
        Surnames = surnames;
        Email = email;
        FinalDate = finalDate;
    }
}
