using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Bids.Commands;
public class CreateBidCommandValidator : BaseValidator<CreateBidCommand>
{
    public CreateBidCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Invalid product");

        RuleFor(x => x.Amount)
            .GreaterThan(0);
    }
}
