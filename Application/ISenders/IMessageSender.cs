using Domain.Interfaces;
using Domain.Models;

namespace Application.ISenders;

public interface IMessageSender
{
    Task SendCollaboratorTempCreationCommandAsync(PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate);
    Task SendUserForCollabCommandAsync(Guid id, PeriodDateTime periodDateTime, string names, string surnames, string email, DateTime finalDate);
}
