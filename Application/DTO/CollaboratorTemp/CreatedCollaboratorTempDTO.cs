using Domain.Models;

namespace Application.DTO.CollaboratorTemp;

public class CreatedCollaboratorTempDTO
{
    public Guid Id { get; set; }
    public PeriodDateTime PeriodDateTime { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; }

    public CreatedCollaboratorTempDTO(Guid id, PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        Id = id;
        PeriodDateTime = periodDateTime;
        Names = names;
        Surnames = surnames;
        Email = email;
        FinalDate = finalDate;
    }
}
