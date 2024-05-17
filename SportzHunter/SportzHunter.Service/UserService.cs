using SportzHunter.Model;
using SportzHunter.Repository.Common;
using SportzHunter.Service.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;

namespace SportzHunter.Service
{
    public class UserService : IUserService
    {
        protected IUserRepository userRepository { get; set; }

        public UserService(IUserRepository _userRepository)
        {
            userRepository = _userRepository;
        }

        public async Task<User> PostLoginUserAsync(User userToLogin)
        {
            return await userRepository.PostLoginUserAsync(userToLogin);
        }

        public async Task<int> PostRegisterUserAsync(User userToRegister, Player newPlayer)
        {
            UpdateProperties(userToRegister, newPlayer);
            return await userRepository.PostRegisterUserAsync(userToRegister, newPlayer);
        }

        public async Task<List<SportCategory>> GetSportCategoriesAsync()
        {
            return await userRepository.GetSportCategoriesAsync();
        }

        public async Task<List<County>> GetCountiesAsync()
        {
            return await userRepository.GetCountiesAsync();
        }

        public async Task<List<PreferredPosition>> GetPrefPositionsAsync()
        {
            return await userRepository.GetPrefPositionsAsync();
        }


        private void UpdateProperties(User user, Player player)
        {
            Guid playerGuid = Guid.NewGuid();
            Guid userGuid = Guid.NewGuid();
            user.Id = userGuid;
            player.Id = playerGuid;
            player.User = user;
            player.CreatedBy = user.Id;
            player.UpdatedBy = user.Id;
            player.DateUpdated = DateTime.UtcNow;
            player.DateCreated = DateTime.UtcNow;
            user.DateCreated = player.DateCreated;
            user.DateUpdated = player.DateUpdated;
            user.CreatedBy = user.Id;
            user.UpdatedBy = user.Id;
            user.PasswordHash = BC.EnhancedHashPassword(user.PasswordHash, 10);
            
        }
    }
}
