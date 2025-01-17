using FluentValidation;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;

namespace HFSolutions.TestDotNet.Application.Validators.UserValidators
{
    public class UserLoginValidator : AbstractValidator<UserLoginDto>
    {
        public UserLoginValidator()
        {
            RuleFor(ul => ul.UserName)
                .MinimumLength(5)
                .MaximumLength(32);

            RuleFor(ul => ul.Password)
                .MinimumLength(8)
                .MaximumLength(30);
        }
    }
}
