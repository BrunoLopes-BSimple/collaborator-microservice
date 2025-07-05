using Domain.Models;

namespace Application.DTO.Collaborators;

public record CollaboratorWithoutUserDTO
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Names { get; set; }
    public string Surnames { get; set; }
    public string Email { get; set; }
    public DateTime DeactivationDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public CollaboratorWithoutUserDTO()
    {
    }

    public CollaboratorWithoutUserDTO(Guid id, Guid userId, string names, string surnames, string email, DateTime deactivationDate, PeriodDateTime periodDateTime)
    {
        Id = id;
        UserId = userId;
        Names = names;
        Surnames = surnames;
        Email = email;
        DeactivationDate = deactivationDate;
        PeriodDateTime = periodDateTime;
    }
}