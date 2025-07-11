using Domain.Models;

namespace Application.DTO;
public class CreatedCollaboratorDTO
{
    public Guid UserId { get; set; }
    public Guid CollaboratorId { get; set; }
    public PeriodDateTime PeriodDateTime { get; set; }

    public CreatedCollaboratorDTO(Guid userId, Guid collaboratorId, PeriodDateTime periodDateTime)
    {
        UserId = userId;
        CollaboratorId = collaboratorId;
        PeriodDateTime = periodDateTime;
    }
}
