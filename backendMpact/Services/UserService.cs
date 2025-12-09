using backendMpact.DTO;
using backendMpact.Models;
using backendMpact.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace backendMpact.Services
{
    public class UserService : IUserService
    {
        // Hardcoded JWT values
        private const string JwtKey = "4bb6d1dfbafb64a681139d1586b6f1160d18159afd57c8c79136d7490630407c"; // must be 32+ chars
        private const string JwtIssuer = "MyBackendApp";
        private const string JwtAudience = "BlazorClient";

        private readonly IUserRepository _repo;
        private readonly IConfiguration _config;
        public UserService(IUserRepository repo)
        {
            _repo = repo;

        }

        //register Admin/Manager
        public async Task CreateUser(RegisterRequest request)
        {
            //  Hash the password here
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(request.Password);
            var hash = sha.ComputeHash(bytes);
            var hashedPassword = Convert.ToBase64String(hash);

            // Convert DTO → User model
            var user = new User
            {

                Email = request.Email,
                FullName=request.FullName,
                LastName=request.LastName,
                Password = hashedPassword,
                Role = "Admin"
            };

            //  Save to database
            _repo.Add(user);
        }

        public Task<bool> UserExists(string email)
        {
            return _repo.UserExists(email);
        }

        // LOGIN
        public async Task<DTO.LoginResponse> Login(LoginRequest request)
        {
            var user = await _repo.GetByEmailAsync(request.Email);

            if (user == null || string.IsNullOrEmpty(user.Password))
                throw new Exception("User not found or password not set");

            // HASH THE INPUT PASSWORD
            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(request.Password);
            var hash = sha.ComputeHash(bytes);
            var hashedPassword = Convert.ToBase64String(hash);

            if (user.Password != hashedPassword)
                throw new Exception("Invalid password");

            // GENERATE JWT
            var token = GenerateJwtToken(user);

            return new DTO.LoginResponse(token, "Login successful");
        }


        // JWT GENERATOR
        private string GenerateJwtToken(User user)
        {
            // Secret key
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Claims
            var claims = new[]
            {
            new Claim("Email", user.Email.ToString()),
            new Claim("UserId", user.Id.ToString()),
            new Claim("Role",user.Role.ToString()),
            new Claim("FullName", user.FullName.ToString()),
             new Claim("lastName", user.LastName.ToString()),
        };

            // Create token
            var token = new JwtSecurityToken(
                issuer: JwtIssuer,
                audience: JwtAudience,
                claims: claims,
                expires: DateTime.Now.AddHours(2), // <-- token expires in 2 hours
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}