using Domain.Contracts;
using Domain.Models;

namespace Application.ISender
{
    public interface IMessageSender
    {
        Task SendCollaboratorCreationCommandAsync(CreateCollaboratorCommand message);
        Task SendUserForCollabCommandAsync(UserForCollabCommandMessage command);
    }
}