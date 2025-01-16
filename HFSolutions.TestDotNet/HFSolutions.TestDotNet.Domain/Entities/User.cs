using System.ComponentModel.DataAnnotations;

namespace HFSolutions.TestDotNet.Domain.Entities
{
    public class User
    {
        [Key]
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public virtual ICollection<UserTask> Tasks { get; set; } = [];
    }
}
