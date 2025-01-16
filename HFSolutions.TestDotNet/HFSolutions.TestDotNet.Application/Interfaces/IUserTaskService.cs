using System.Linq.Expressions;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Domain.Entities;

namespace HFSolutions.TestDotNet.Application.Interfaces
{
    public interface IUserTaskService
    {
        Task<UserTaskDto?> CreateAsync(CreateUserTaskDto createUserTaskDto);
        Task<IEnumerable<UserTaskDto>> ReadAllAsync(Expression<Func<UserTask, bool>>? predicate = null);
        Task<UserTaskDto?> ReadAsync(int id);
        Task<UserTaskDto?> UpdateAsync(int id, UpdateUserTaskDto updateUserTaskDto);
        Task<int> DeleteAsync(int id);
    }
}
