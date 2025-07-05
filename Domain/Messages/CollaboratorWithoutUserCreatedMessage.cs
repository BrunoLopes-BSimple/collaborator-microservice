using Domain.Models;

namespace Domain.Messages;

public record CollaboratorWithoutUserCreatedMessage(Guid Id, string Names, string Surnames, string Email, DateTime DeactivationDate);