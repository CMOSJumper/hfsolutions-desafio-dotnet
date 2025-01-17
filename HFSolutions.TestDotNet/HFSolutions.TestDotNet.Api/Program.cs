using HFSolutions.TestDotNet.Api.Extensions;
using HFSolutions.TestDotNet.Application.Configuration;
using HFSolutions.TestDotNet.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var jwtOptionsSection = builder.Configuration.GetSection("JwtOptions");

builder.Services.Configure<JwtOptions>(jwtOptionsSection);

builder.Services.AddJwtAuth(jwtOptionsSection);

builder.Services.AddControllers();
builder.Services.AddDbContext<UserTasksContext>(options =>
{
    string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

    options.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(typeof(UserTasksContext).Assembly.GetName().Name));
});

builder.Services.AddServices()
    .AddValidators();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerWithAuthentication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
