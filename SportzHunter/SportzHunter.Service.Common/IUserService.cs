using SportzHunter.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SportzHunter.Service.Common
{
    public interface IUserService
    {
        Task<int> PostRegisterUserAsync(User userToRegister, Player newPlayer);
        Task<User> PostLoginUserAsync(User userToLogin);
        Task<List<SportCategory>> GetSportCategoriesAsync();
        Task<List<PreferredPosition>> GetPrefPositionsAsync();
        Task<List<County>> GetCountiesAsync();

    }
}
