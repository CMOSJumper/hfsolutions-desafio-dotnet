using HFSolutions.TestDotNet.Tests.Application.Interfaces;
using HFSolutions.TestDotNet.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace HFSolutions.TestDotNet.Tests.UserTests
{
    [Collection(nameof(ServiceProviderCollection))]
    public class UserTasksTests
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IUserServiceTests _userServiceTests;

        public UserTasksTests(ITestOutputHelper testOutput, ServiceProviderFixture serviceProviderFixture)
        {
            _testOutput = testOutput;
            _serviceProvider = serviceProviderFixture.ServiceProvider;
            _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            _userServiceTests = _serviceProvider.GetRequiredService<IUserServiceTests>();
        }

        [Theory]
        [InlineData("testusername3", "testusername3")]
        public async Task Should_Get_Logged_User_Tasks(string username, string password)
        {
            var userTasks = await _userServiceTests.GetUserTasks(username, password);

            Assert.NotEmpty(userTasks ?? []);
        }
    }
}
