
namespace Domain.Commands;

public record CreateUserFromCollaboratorCommand(string Names, string Surnames, string Email, DateTime DeactivationDate);
