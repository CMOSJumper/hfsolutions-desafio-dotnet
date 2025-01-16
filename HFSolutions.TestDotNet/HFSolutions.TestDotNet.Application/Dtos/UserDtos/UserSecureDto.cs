using HFSolutions.TestDotNet.Domain.Entities;
using Mapster;

namespace HFSolutions.TestDotNet.Application.Dtos.UserDtos
{
    public class UserSecureDto
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public static implicit operator UserSecureDto?(User? user) => user?.Adapt<UserSecureDto>();
    }
}
