/* using MassTransit;
using Application.Services;
using Domain.Messages;
using Application.Interfaces;

namespace WebApi.Consumers
{
    public class UserFromCollaboratorCreatedConsumer : IConsumer<UserFromCollaboratorCreatedMessage>
    {
        private readonly UserService _userService;
        private readonly ICollaboratorService _collaboratorService;
        public UserFromCollaboratorCreatedConsumer(UserService userService, ICollaboratorService collaboratorService)
        {
            _userService = userService;
            _collaboratorService = collaboratorService;
        }

        public async Task Consume(ConsumeContext<UserFromCollaboratorCreatedMessage> context)
        {
            var msg = context.Message;
            await _collaboratorService.AddUserIdForCollaboratorAsync(msg.Id, msg.CollaboratorId);
            await _userService.AddUserReferenceAsync(msg.Id);
        }
    }
} */