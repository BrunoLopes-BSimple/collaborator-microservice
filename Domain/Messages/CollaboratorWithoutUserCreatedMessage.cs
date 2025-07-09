using Domain.Models;

namespace Domain.Messages;

public record CollaboratorWithoutUserStartSagaMessage(Guid Id, string Names, string Surnames, string Email, DateTime DeactivationDate, PeriodDateTime PeriodDateTime);