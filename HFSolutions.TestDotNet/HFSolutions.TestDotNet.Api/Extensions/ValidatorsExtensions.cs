using FluentValidation;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Application.Validators.UserTaskValidators;
using HFSolutions.TestDotNet.Application.Validators.UserValidators;

namespace HFSolutions.TestDotNet.Api.Extensions
{
    public static class ValidatorsExtensions
    {
        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
            services.AddScoped<IValidator<UserSecureDto>, UserSecureDtoValidator>();
            services.AddScoped<IValidator<CreateUserTaskDto>, CreateUserTaskDtoValidator>();
            services.AddScoped<IValidator<UpdateUserTaskDto>, UpdateUserTaskDtoValidator>();

            return services;
        }
    }
}
