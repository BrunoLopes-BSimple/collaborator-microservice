using Domain.Models;

namespace Domain.Messages
{
    public record UserFromCollaboratorCreatedMessage(Guid Id, Guid CollaboratorId, string Names, string Surnames, string Email, PeriodDateTime PeriodDateTime);
}