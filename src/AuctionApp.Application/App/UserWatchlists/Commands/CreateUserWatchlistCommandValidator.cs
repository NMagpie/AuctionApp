using Application.Common.Validation;
using FluentValidation;

namespace Application.App.UserWatchlists.Commands;
public class CreateUserWatchlistCommandValidator : BaseValidator<CreateUserWatchlistCommand>
{
    public CreateUserWatchlistCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Invalid product");
    }
}
