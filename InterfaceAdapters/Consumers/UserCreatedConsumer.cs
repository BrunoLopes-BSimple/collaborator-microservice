using MassTransit;
using Application.Services;
using Domain.Messages;

namespace InterfaceAdapters.Consumers
{
    public class UserCreatedConsumer : IConsumer<UserCreatedMessage>
    {
        private readonly UserService _userService;

        public UserCreatedConsumer(UserService userService)
        {
            _userService = userService;
        }

        public async Task Consume(ConsumeContext<UserCreatedMessage> context)
        {
            var userId = context.Message.Id;
            await _userService.AddUserReferenceAsync(userId);
        }
    }
}