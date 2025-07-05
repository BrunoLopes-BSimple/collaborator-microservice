using Domain.Models;

namespace Application.DTO.Collaborators;

public record CreateCollaboratorWithoutUserDTO
{
    public string Names { get; set; }
    public string Surnames { get; set; }
    public string Email { get; set; }
    public DateTime DeactivationDate { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

}