using FluentValidation;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;

namespace HFSolutions.TestDotNet.Application.Validators.UserValidators
{
    public class CreateUserDtoValidator : AbstractValidator<CreateUserDto>
    {
        public CreateUserDtoValidator()
        {
            RuleFor(cud => cud.UserName)
                .MinimumLength(5)
                .MaximumLength(32);

            RuleFor(cud => cud.Email)
                .EmailAddress()
                .MaximumLength(254);

            RuleFor(cud => cud.Password)
                .MinimumLength(8)
                .MaximumLength(30);
        }
    }
}
