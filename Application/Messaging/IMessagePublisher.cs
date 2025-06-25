using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Interfaces;

namespace Application.Messaging
{
    public interface IMessagePublisher
    {
        Task PublishCollaboratorCreatedAsync(ICollaborator collaborator);
        Task PublishCollaboratorUpdatedAsync(ICollaborator collaborator);
    }
}