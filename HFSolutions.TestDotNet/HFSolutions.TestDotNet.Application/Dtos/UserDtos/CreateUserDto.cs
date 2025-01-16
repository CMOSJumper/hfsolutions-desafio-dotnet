using HFSolutions.TestDotNet.Domain.Entities;
using Mapster;

namespace HFSolutions.TestDotNet.Application.Dtos.UserDtos
{
    public class CreateUserDto
    {
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public static implicit operator User(CreateUserDto createUserDto) => createUserDto.Adapt<User>();
        public static implicit operator CreateUserDto(User user) => user.Adapt<CreateUserDto>();
    }
}
