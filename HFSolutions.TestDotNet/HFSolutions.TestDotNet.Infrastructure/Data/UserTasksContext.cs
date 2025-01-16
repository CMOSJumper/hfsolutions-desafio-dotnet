using HFSolutions.TestDotNet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HFSolutions.TestDotNet.Infrastructure.Data
{
    public class UserTasksContext(DbContextOptions<UserTasksContext> options) : DbContext(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string server = "192.168.2.125";
                string serverPort = "1433";
                string database = "UserTasks";
                string userId = "hfsolutions";
                string password = "hfsolutions";

                optionsBuilder.UseSqlServer($"Server=tcp:{server},{serverPort};" +
                    $"Initial Catalog={database};Persist Security Info=False;" +
                    $"User ID={userId};Password={password};" +
                    $"MultipleActiveresultSets=False;Encrypt=True;" +
                    $"TrustServerCertificate=True;Connect Timeout=60;");
            }
        }

        public DbSet<User> User { get; set; }
        public DbSet<TaskState> TaskState { get; set; }
        public DbSet<UserTask> UserTask { get; set; }
    }
}
