using HFSolutions.TestDotNet.Tests.Application.Interfaces;
using HFSolutions.TestDotNet.Tests.Fixtures;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace HFSolutions.TestDotNet.Tests.UserTests
{
    [Collection(nameof(ServiceProviderCollection))]
    public class LoginTests
    {
        private readonly ITestOutputHelper _testOutput;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly IUserServiceTests _userServiceTests;

        public LoginTests(ITestOutputHelper testOutput, ServiceProviderFixture serviceProviderFixture)
        {
            _testOutput = testOutput;
            _serviceProvider = serviceProviderFixture.ServiceProvider;
            _configuration = _serviceProvider.GetRequiredService<IConfiguration>();
            _userServiceTests = _serviceProvider.GetRequiredService<IUserServiceTests>();
        }

        [Theory]
        [InlineData("testusername3", "testusername3")]
        public async Task Login_Should_Get_JWT_Token(string username, string password)
        {
            string token = await _userServiceTests.Login(username, password);

            Assert.NotEmpty(token);
        }
    }
}
