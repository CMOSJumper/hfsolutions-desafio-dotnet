using FluentValidation;
using HFSolutions.TestDotNet.Application.Dtos.UserTaskDto;

namespace HFSolutions.TestDotNet.Application.Validators.UserTaskValidators
{
    public class CreateUserTaskDtoValidator : AbstractValidator<CreateUserTaskDto>
    {
        public CreateUserTaskDtoValidator()
        {
            RuleFor(ut => ut.Description)
                .NotEmpty()
                .MaximumLength(300);

            RuleFor(ut => ut.ExpirationDate)
                .GreaterThan(DateTime.Now);
        }
    }
}
