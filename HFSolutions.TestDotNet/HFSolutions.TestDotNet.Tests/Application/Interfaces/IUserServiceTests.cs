using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;

namespace HFSolutions.TestDotNet.Tests.Application.Interfaces
{
    public interface IUserServiceTests
    {
        Task<string> Login(string username, string password);
        Task<List<UserTaskDto>?> GetUserTasks(string username, string password);
    }
}
