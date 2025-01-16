using System.Linq.Expressions;
using HFSolutions.TestDotNet.Abstractions.Crud;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HFSolutions.TestDotNet.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserSecureDto?> CreateAsync(CreateUserDto createUserDto);
        Task<IEnumerable<UserSecureDto>> ReadAllAsync(Expression<Func<User, bool>>? predicate = null);
        Task<UserSecureDto?> ReadAsync(int id);
        Task<UserSecureDto?> UpdateAsync(int id, UserSecureDto userSecureDto);
        Task<int> DeleteAsync(int id);
    }
}
