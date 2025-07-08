using Domain.Models;

namespace Domain.Messages;

public record CollaboratorWithoutUserStartSagaMessage(Guid CorrelationId, string Names, string Surnames, string Email, DateTime DeactivationDate, PeriodDateTime PeriodDateTime);