using HFSolutions.TestDotNet.Tests.Application.Interfaces;
using HFSolutions.TestDotNet.Tests.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HFSolutions.TestDotNet.Tests.Fixtures
{
    public class ServiceProviderFixture
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ServiceProviderFixture()
        {
            var configuration = CreateConfiguration();

            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging();
            serviceCollection.AddSingleton(configuration);
            serviceCollection.AddHttpClient<IUserServiceTests, UserServiceTests>(client =>
            {
                string apiUrl = configuration.GetRequiredSection("ApiUrls:Base").Get<string>()!;

                client.BaseAddress = new Uri(apiUrl);
            });

            var serviceProvider = serviceCollection.BuildServiceProvider();

            ServiceProvider = serviceProvider;
        }

        private static IConfiguration CreateConfiguration()
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            return configuration;
        }
    }
}
