using FluentValidation;
using HFSolutions.TestDotNet.Application.Dtos.UserDtos;

namespace HFSolutions.TestDotNet.Application.Validators.UserValidators
{
    public class UserSecureDtoValidator : AbstractValidator<UserSecureDto>
    {
        public UserSecureDtoValidator()
        {
            RuleFor(usd => usd.UserId)
                .GreaterThan(0);

            RuleFor(usd => usd.UserName)
                    .MinimumLength(5)
                    .MaximumLength(32);

            RuleFor(usd => usd.Email)
                .EmailAddress()
                .MaximumLength(254);
        }
    }
}
