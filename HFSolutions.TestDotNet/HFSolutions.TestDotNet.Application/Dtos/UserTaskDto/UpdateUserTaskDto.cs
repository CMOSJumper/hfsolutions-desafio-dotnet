using HFSolutions.TestDotNet.Domain.Entities;
using Mapster;

namespace HFSolutions.TestDotNet.Application.Dtos.UserTaskDto
{
    public class UpdateUserTaskDto
    {
        public int UserTaskId { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime ExpirationDate { get; set; }
        public int TaskStateId { get; set; }
        public int UserId { get; set; }

        public static implicit operator UserTask(UpdateUserTaskDto updateUserTaskDto) => updateUserTaskDto.Adapt<UserTask>();
        public static implicit operator UpdateUserTaskDto(UserTask userTask) => userTask.Adapt<UpdateUserTaskDto>();
    }
}
