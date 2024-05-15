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

        RuleFor(x => x.UserName)
            .NotEmpty()
            .WithMessage("Usename must be present");

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage("Email must be present");

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("Password must be present");

        RuleFor(x => x.UserName)
            .Length(4, 64).
            WithMessage("Username length must be between 4 and 64 characters");
    }
}
