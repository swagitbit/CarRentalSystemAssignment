using CarRentalSystemAssignment.Models;
using CarRentalSystemAssignment.Repositories;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BCrypt.Net;

namespace CarRentalSystemAssignment.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UserService(UserRepository userRepository, IConfiguration configuration)
        {
            _userRepository = userRepository;
            _configuration = configuration;
        }

        // Register a new user
        public async Task<string> RegisterUser(UserModel user)
        {
            var existingUser = await _userRepository.GetUserByEmail(user.Email);

            if (existingUser != null)
                return "User with this email already exists.";

            user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

            await _userRepository.AddUser(user);
            return "User registered successfully.";
        }

        // Authenticate user and generate JWT token
        public async Task<string?> AuthenticateUser(string email, string password)
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
                return null;

            // Generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1), 
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
