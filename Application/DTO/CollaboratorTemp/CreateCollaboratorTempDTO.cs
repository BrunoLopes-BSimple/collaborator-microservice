using Domain.Models;

namespace Application.DTO.CollaboratorTemp;

public class CreateCollaboratorTempDTO
{
    public PeriodDateTime PeriodDateTime { get; }
    public string Names { get; }
    public string Surnames { get; }
    public string Email { get; }
    public DateTime FinalDate { get; }

    public CreateCollaboratorTempDTO(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate)
    {
        PeriodDateTime = periodDateTime;
        Names = names;
        Surnames = surnames;
        Email = email;
        FinalDate = finalDate;
    }
}
