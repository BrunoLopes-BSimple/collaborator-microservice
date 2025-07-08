using Domain.Models;

namespace Application.DTO.Collaborators;

public record CreateCollaboratorWithoutUserDTO
{
    public string Names { get; set; }
    public string Surnames { get; set; }
    public string Email { get; set; }
    public DateTime DeactivationDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public CreateCollaboratorWithoutUserDTO(string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime)
    {
        Names = names;
        Surnames = surnames;
        Email = email;
        DeactivationDate = deactivationDate;
        PeriodDateTime = periodDateTime;
    }

}