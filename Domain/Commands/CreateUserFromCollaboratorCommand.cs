using Domain.Models;

namespace Domain.Commands;

public record CreateUserFromCollaboratorCommand(Guid CorrelationId, string InstanceId, Guid Id, string Names, string Surnames, string Email, DateTime DeactivationDate);
