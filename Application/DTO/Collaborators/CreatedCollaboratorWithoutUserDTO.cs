using Domain.Models;

namespace Application.DTO.Collaborators;

public record CreatedCollaboratorWithoutUserDTO
{
    public Guid Id { get; }
    public string Names { get; set; }
    public string Surnames { get; set; }
    public string Email { get; set; }
    public DateTime DeactivationDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public CreatedCollaboratorWithoutUserDTO()
    {
    }

    public CreatedCollaboratorWithoutUserDTO(Guid id, string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime)
    {
        Id = id;
        Names = names;
        Surnames = surnames;
        Email = email;
        DeactivationDate = deactivationDate;
        PeriodDateTime = periodDateTime;
    }
}