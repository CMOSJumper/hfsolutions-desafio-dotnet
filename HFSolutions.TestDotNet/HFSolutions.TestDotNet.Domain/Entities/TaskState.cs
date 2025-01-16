using System.ComponentModel.DataAnnotations;

namespace HFSolutions.TestDotNet.Domain.Entities
{
    public class TaskState
    {
        [Key]
        public int TaskStateId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
