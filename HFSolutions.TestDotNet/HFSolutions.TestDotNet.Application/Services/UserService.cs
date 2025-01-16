using System.Linq.Expressions;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Extensions;
using HFSolutions.TestDotNet.Application.Interfaces;
using HFSolutions.TestDotNet.Domain.Entities;
using HFSolutions.TestDotNet.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HFSolutions.TestDotNet.Application.Services
{
    public class UserService(ILogger<UserService> logger, UserTasksContext context, ICustomPasswordHasher passwordHasher) : IUserService
    {
        private readonly ILogger<UserService> _logger = logger;
        private readonly UserTasksContext _context = context;
        private readonly ICustomPasswordHasher _passwordHasher = passwordHasher;

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
