using Domain.Models;

namespace Application.DTO.Collaborators;

public record CollabWithoutUserDTO(string Names, string Surnames, string Email, DateTime UserDeactivationDate, PeriodDateTime CollaboratorPeriod);
