using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Users.Commands;
public class CreateUserCommandValidator : BaseValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Usename must be present");

        RuleFor(x => x.Username)
            .Length(4, 64).
            WithMessage("Username length must be between 4 and 64 characters");
    }
}
