using SportzHunter.Common;
using SportzHunter.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Repository.Common
{
    public interface IUserRepository
    {
        Task<int> PostRegisterUserAsync(User userToRegister, Player newPlayer);
        Task<User> PostLoginUserAsync(User userToLogin);
        Task<User> GetUserById(Guid id);
        Task<List<SportCategory>> GetSportCategoriesAsync();
        Task<List<PreferredPosition>> GetPrefPositionsAsync();
        Task<List<County>> GetCountiesAsync();
        Task<Guid> GetRoleByNameAsync(string roleName);
        Task<PagedList<User>> GetUsersAsync(Paging paging, Sorting sorting);
    }
}
