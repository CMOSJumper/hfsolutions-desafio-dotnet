using System.Linq.Expressions;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Application.Extensions;
using HFSolutions.TestDotNet.Application.Interfaces;
using HFSolutions.TestDotNet.Domain.Entities;
using HFSolutions.TestDotNet.Infrastructure.Data;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HFSolutions.TestDotNet.Application.Services
{
    public class UserTaskService(ILogger<UserTaskService> logger, UserTasksContext context) : IUserTaskService
    {
        private readonly ILogger<UserTaskService> _logger = logger;
        private readonly UserTasksContext _context = context;

        public async Task<UserTaskDto?> CreateAsync(CreateUserTaskDto createUserTaskDto)
        {
            try
            {
                UserTask newUserTask = createUserTaskDto;
                await _context.UserTask.AddAsync(newUserTask);
                await _context.SaveChangesAsync();

                var userTaskCreated = await ReadAsync(newUserTask.UserTaskId);

                return userTaskCreated;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to create a new user task.");

                return null;
            }
        }

        public async Task<IEnumerable<UserTaskDto>> ReadAllAsync(Expression<Func<UserTask, bool>>? predicate = null)
        {
            try
            {
                var userTasksDto = await _context.UserTask
                    .Include(ut => ut.TaskState)
                    .Include(ut => ut.User)
                    .WhereIf(predicate != null, predicate!)
                    .Select(ut => ut.Adapt<UserTaskDto>())
                    .ToListAsync();

                return userTasksDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to get all user tasks.");

                return [];
            }
        }

        public async Task<UserTaskDto?> ReadAsync(int id)
        {
            try
            {
                UserTaskDto? userTaskDto = await _context.UserTask
                    .Include(ut => ut.TaskState)
                    .Include(ut => ut.User)
                    .FirstOrDefaultAsync(ut => ut.UserTaskId == id);

                return userTaskDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to get the specificied user task.");

                return null;
            }
        }

        public async Task<UserTaskDto?> UpdateAsync(int id, UpdateUserTaskDto updateUserTaskDto)
        {
            try
            {
                var userTask = await _context.UserTask.FirstOrDefaultAsync(ut => ut.UserTaskId == id)
                    ?? throw new NullReferenceException("The user task specified does not exist.");

                updateUserTaskDto.UserId = userTask.UserId;

                _context.Entry(userTask).CurrentValues.SetValues(updateUserTaskDto);
                await _context.SaveChangesAsync();

                var updatedUserTask = await ReadAsync(userTask.UserTaskId);

                return updatedUserTask;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to update the specified user task.");

                return null;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            try
            {
                var userTask = await _context.UserTask.FirstOrDefaultAsync(ut => ut.UserTaskId == id)
                    ?? throw new NullReferenceException("The user task specified does not exist.");

                _context.UserTask.Remove(userTask);
                int result = await _context.SaveChangesAsync();
                id = result > 0 ? id : 0;

                return id;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error ocurred trying to delete the specified user task.");

                return 0;
            }
        }
    }
}
