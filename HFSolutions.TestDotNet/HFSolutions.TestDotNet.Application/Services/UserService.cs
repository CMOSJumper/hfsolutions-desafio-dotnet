using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using HFSolutions.TestDotNet.Application.Configuration;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Extensions;
using HFSolutions.TestDotNet.Application.Interfaces;
using HFSolutions.TestDotNet.Domain.Entities;
using HFSolutions.TestDotNet.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HFSolutions.TestDotNet.Application.Services
{
    public class UserService(ILogger<UserService> logger,
        IOptions<JwtOptions> jwtOptions,
        UserTasksContext context,
        ICustomPasswordHasher passwordHasher) : IUserService
    {
        private readonly ILogger<UserService> _logger = logger;
        private readonly IOptions<JwtOptions> _jwtOptions = jwtOptions;
        private readonly UserTasksContext _context = context;
        private readonly ICustomPasswordHasher _passwordHasher = passwordHasher;

        private string GenerateJWTToken(int userId, string username)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new Claim(JwtRegisteredClaimNames.Name, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.Value.IssuerSigningKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtOptions.Value.ValidIssuer,
                audience: _jwtOptions.Value.ValidAudience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_jwtOptions.Value.ExpirationMinutes),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<string?> Login(UserLoginDto userLoginDto)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == userLoginDto.UserName) 
                    ?? throw new NullReferenceException("The given user doe snot exist.");

                bool loginResult = _passwordHasher.VerifyPasswordHash(user.Password, userLoginDto.Password);

                if (!loginResult)
                {
                    throw new Exception("Username or password incorrect.");
                }

                var token = GenerateJWTToken(user.UserId, user.UserName);

                return token;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to login.");

                return null;
            }
        }

        public async Task<UserSecureDto?> CreateAsync(CreateUserDto createUserDto)
        {
            try
            {
                createUserDto.Password = _passwordHasher.HashPassword(createUserDto.Password);
                User newUser = createUserDto;
                await _context.User.AddAsync(newUser);
                await _context.SaveChangesAsync();

                return newUser;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to create a new user.");

                return null;
            }
        }

        public async Task<IEnumerable<UserSecureDto>> ReadAllAsync(Expression<Func<User, bool>>? predicate = null)
        {
            try
            {
                var usersSecureDto = await _context.User
                    .WhereIf(predicate != null, predicate!)
                    .Select(u => u.Adapt<UserSecureDto>())
                    .ToListAsync();

                return usersSecureDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to get all users.");

                return [];
            }
        }

        public async Task<UserSecureDto?> ReadAsync(int id)
        {
            try
            {
                UserSecureDto? userSecureDto = await _context.User.FirstOrDefaultAsync(u => u.UserId == id);

                return userSecureDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to get the specificied user.");

                return null;
            }
        }

        public async Task<UserSecureDto?> UpdateAsync(int id, UserSecureDto userSecureDto)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id)
                    ?? throw new NullReferenceException("The user specified does not exist.");

                _context.Entry(user).CurrentValues.SetValues(userSecureDto);
                await _context.SaveChangesAsync();

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to update the specified user.");

                return null;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserId == id)
                    ?? throw new NullReferenceException("The user specified does not exist.");

                _context.User.Remove(user);
                int result = await _context.SaveChangesAsync();
                id = result > 0 ? id : 0;

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to delete the specified user.");

                return 0;
            }
        }
    }
}
