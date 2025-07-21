using Domain.Models;

namespace Application.DTO;

public record CreatedCollaboratorDTO(Guid UserId, Guid CollaboratorId, PeriodDateTime PeriodDateTime);
