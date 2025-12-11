using Azure.Core;
using backendMpact.DTO;
using backendMpact.Models;

namespace backendMpact.Services
{
    public interface IUserService
    {
        Task CreateUser(RegisterRequest request);
        Task<bool> UserExists(string email);
        Task<DTO.LoginResponse> Login(LoginRequest request);
        Task<List<AllUsers>> GetAllUsers();
    }
}
