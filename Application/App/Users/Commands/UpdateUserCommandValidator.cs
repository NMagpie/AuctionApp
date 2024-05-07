using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Users.Commands;
public class UpdateUserCommandValidator : BaseValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage("Usename must be present");

        RuleFor(x => x.Username)
            .Length(4, 64).
            WithMessage("Username length must be between 4 and 64 characters");
    }
}
