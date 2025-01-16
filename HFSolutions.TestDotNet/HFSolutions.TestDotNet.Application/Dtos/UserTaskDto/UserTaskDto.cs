using HFSolutions.TestDotNet.Application.Dtos.UserDtos;
using HFSolutions.TestDotNet.Domain.Entities;
using Mapster;

namespace HFSolutions.TestDotNet.Application.Dtos.UserTaskDto
{
    public class UserTaskDto
    {
        public int UserTaskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public TaskStateDto TaskState { get; set; } = default!;
        public UserSecureDto User { get; set; } = default!;

        public static implicit operator UserTaskDto?(UserTask? userTask) => userTask?.Adapt<UserTaskDto>();
    }
}
