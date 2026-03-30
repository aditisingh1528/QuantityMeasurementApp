using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using QuantityMeasurementBusinessLayer.Interfaces;
using QuantityMeasurementModel.DTOs;
using QuantityMeasurementModel.Entities;
using QuantityMeasurementRepository.EFCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using QuantityMeasurementModel.Exceptions;
namespace QuantityMeasurementBusinessLayer.Services
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly QuantityMeasurementDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthServiceImpl(QuantityMeasurementDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public User Register(AuthRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username))
                throw new ValidationException("Username cannot be empty.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("Password cannot be empty.");

            var username = request.Username.Trim();
            var password = request.Password;

            if (password.Length < 3 || password.Length > 10)
                throw new ValidationException("Password must be between 3 and 10 characters long.");
            
            if (!Regex.IsMatch(password, @"[A-Z]"))
                throw new ValidationException("Password must contain at least one uppercase letter.");
            
            if (!Regex.IsMatch(password, @"[\W_]"))
                throw new ValidationException("Password must contain at least one special character.");

            if (_context.Users.Any(u => u.Username.ToLower() == username.ToLower()))
            {
                throw new UserAlreadyExistsException("User already exists.");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password)
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return user;
        }

        public string Login(AuthRequestDTO request)
        {
            if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("Username and password are required.");

            var username = request.Username.Trim();
            var user = _context.Users.AsEnumerable().SingleOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new ValidationException("Invalid username or password.");
            }

            return GenerateJwtToken(user);
        }

        private string GenerateJwtToken(User user)
        {
            var keyStr = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(keyStr)) throw new ArgumentNullException("Jwt:Key is not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(keyStr));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim("id", user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
