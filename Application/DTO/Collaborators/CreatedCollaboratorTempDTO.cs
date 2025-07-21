using Domain.Models;

namespace Application.DTO.Collaborators;

public record CreatedCollaboratorTempDTO(Guid Id, PeriodDateTime PeriodDateTime, string Names, string Surnames, string Email, DateTime FinalDate);