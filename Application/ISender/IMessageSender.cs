using Domain.Contracts;
using Domain.Models;

namespace Application.ISender
{
    public interface IMessageSender
    {
        Task SendCollaboratorCreationCommandAsync(CreateCollaboratorCommand message);
        Task SendUserForCollabCommandAsync(Guid id, PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate);
    }
}