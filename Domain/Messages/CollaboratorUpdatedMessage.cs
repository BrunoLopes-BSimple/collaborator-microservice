using Domain.Models;

namespace Domain.Messages
{
    public record CollaboratorUpdatedMessage(Guid Id, Guid UserId, PeriodDateTime PeriodDateTime);
}