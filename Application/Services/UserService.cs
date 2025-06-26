using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;

namespace Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserFactory _userFactory;

        public UserService(IUserRepository userRepository, IUserFactory userFactory)
        {
            _userRepository = userRepository;
            _userFactory = userFactory;
        }

        public async Task<IUser?> AddUserReferenceAsync(Guid userId)
        {
            var newUser = await _userFactory.Create(userId);

            if (newUser == null) return null;

            return await _userRepository.AddAsync(newUser);
        }
    }
}