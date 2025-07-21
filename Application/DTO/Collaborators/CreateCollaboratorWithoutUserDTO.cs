using Domain.Models;

namespace Application.DTO.Collaborators;

public record CreateCollaboratorWithoutUserDTO(string Names, string Surnames, string Email, DateTime UserDeactivationDate, PeriodDateTime CollaboratorPeriod);