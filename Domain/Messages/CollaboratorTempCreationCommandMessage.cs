using Domain.Models;

namespace Domain.Messages;

public record CollaboratorTempCreationCommandMessage(PeriodDateTime PeriodDateTime, string Names, string Surnames, string Email, DateTime FinalDate);
