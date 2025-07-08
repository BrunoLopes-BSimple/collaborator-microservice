using Domain.Models;

namespace Domain.Messages
{
    public record UserFromCollaboratorCreatedMessage(Guid CorrelationId, Guid UserId, Guid CollaboratorId, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);
}