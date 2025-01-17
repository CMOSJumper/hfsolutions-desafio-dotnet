using System.Linq.Expressions;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Domain.Entities;

namespace HFSolutions.TestDotNet.Application.Interfaces
{
    public interface IUserService
    {
        Task<string?> Login(UserLoginDto userLoginDto);
        Task<UserSecureDto?> CreateAsync(CreateUserDto createUserDto);
        Task<IEnumerable<UserSecureDto>> ReadAllAsync(Expression<Func<User, bool>>? predicate = null);
        Task<UserSecureDto?> ReadAsync(int id);
        Task<UserSecureDto?> UpdateAsync(int id, UserSecureDto userSecureDto);
        Task<int> DeleteAsync(int id);
    }
}
