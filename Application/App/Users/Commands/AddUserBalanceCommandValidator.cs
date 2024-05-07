using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Users.Commands;
public class AddUserBalanceCommandValidator : BaseValidator<AddUserBalanceCommand>
{
    public AddUserBalanceCommandValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be positive");
    }
}
