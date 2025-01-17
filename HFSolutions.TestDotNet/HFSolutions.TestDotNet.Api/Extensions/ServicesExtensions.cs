using HFSolutions.TestDotNet.Application.Interfaces;
using HFSolutions.TestDotNet.Application.Services;

namespace HFSolutions.TestDotNet.Api.Extensions
{
    public static class ServicesExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<ICustomPasswordHasher, PasswordHasher>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserTaskService, UserTaskService>();

            return services;
        }
    }
}
