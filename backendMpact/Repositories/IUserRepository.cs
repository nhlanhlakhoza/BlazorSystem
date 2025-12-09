using backendMpact.Models;

namespace backendMpact.Repositories
{
    public interface IUserRepository
    {
        void Add(User user);
        Task<User> GetByEmailAsync(string email);
        Task<bool> UserExists(string email);
   
    }
}

