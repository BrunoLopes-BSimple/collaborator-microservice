using Domain.Models;

namespace Application.DTO;

public record CreateCollaboratorDTO(Guid UserId, PeriodDateTime PeriodDateTime);
