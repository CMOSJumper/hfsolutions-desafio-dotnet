using FluentValidation;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;
using HFSolutions.TestDotNet.Application.Interfaces;
using HFSolutions.TestDotNet.Application.Services;
using HFSolutions.TestDotNet.Application.Validators.UserTaskValidators;
using HFSolutions.TestDotNet.Application.Validators.UserValidators;
using HFSolutions.TestDotNet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<UserTasksContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

    options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(UserTasksContext).Assembly.GetName().Name));
});

builder.Services.AddScoped<ICustomPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserDtoValidator>();
builder.Services.AddScoped<IValidator<UserSecureDto>, UserSecureDtoValidator>();
builder.Services.AddScoped<IUserTaskService, UserTaskService>();
builder.Services.AddScoped<IValidator<CreateUserTaskDto>, CreateUserTaskDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateUserTaskDto>, UpdateUserTaskDtoValidator>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
