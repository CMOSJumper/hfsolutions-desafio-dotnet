using HFSolutions.TestDotNet.Domain.Entities;
using Mapster;

namespace HFSolutions.TestDotNet.Application.Dtos
{
    public class TaskStateDto
    {
        public int TaskStateId { get; set; }
        public string Name { get; set; } = string.Empty;

        public static implicit operator TaskState(TaskStateDto taskStateDto) => taskStateDto.Adapt<TaskState>();
        public static implicit operator TaskStateDto(TaskState taskState) => taskState.Adapt<TaskStateDto>();
    }
}
