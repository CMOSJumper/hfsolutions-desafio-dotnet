using System.ComponentModel.DataAnnotations;

namespace HFSolutions.TestDotNet.Domain.Entities
{
    public class UserTask
    {
        [Key]
        public int UserTaskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int TaskStateId { get; set; }
        public TaskState TaskState { get; set; } = default!;
        public int UserId { get; set; }
        public User User { get; set; } = default!;
    }
}
