using Domain.Models;

namespace Application.DTO.Collaborators;

public record CollaboratorDTO(Guid Id, Guid UserId, PeriodDateTime PeriodDateTime);
