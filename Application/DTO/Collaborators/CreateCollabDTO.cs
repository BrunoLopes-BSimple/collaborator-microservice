using Domain.Models;

namespace Application.DTO.Collaborators;

public class CreateCollabDTO
{
    // Normal collaborator Creation
    public Guid UserId { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }
}
