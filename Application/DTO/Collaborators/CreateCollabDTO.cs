using Domain.Models;

namespace Application.DTO.Collaborators;

public class CreateCollabDTO
{
    // Case 1: Normal creation
    public Guid? UserId { get; set; }

    // Case 2: Create user + temp collaborator
    public string? Names { get; set; }
    public string? Surnames { get; set; }
    public string? Email { get; set; }
    public DateTime? FinalDate { get; set; }

    // Common
    public required PeriodDateTime PeriodDateTime { get; set; }
}
