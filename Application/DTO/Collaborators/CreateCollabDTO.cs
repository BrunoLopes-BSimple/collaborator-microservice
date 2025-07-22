using Domain.Models;

namespace Application.DTO.Collaborators;

public record CreateCollabDTO(Guid? UserId, PeriodDateTime PeriodDateTime, string Names, string Surnames, string Email, DateTime FinalDate);
