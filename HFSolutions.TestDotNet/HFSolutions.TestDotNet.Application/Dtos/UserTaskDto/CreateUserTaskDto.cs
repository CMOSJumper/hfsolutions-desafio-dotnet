using HFSolutions.TestDotNet.Domain.Entities;
using Mapster;

namespace HFSolutions.TestDotNet.Application.Dtos.UserTaskDto
{
    public class CreateUserTaskDto
    {
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int TaskStateId { get; set; }
        public int UserId { get; set; }

        public static implicit operator UserTask(CreateUserTaskDto createUserTaskDto) => createUserTaskDto.Adapt<UserTask>();
        public static implicit operator CreateUserTaskDto(UserTask userTask) => userTask.Adapt<CreateUserTaskDto>();
    }
}
