using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Bids.Commands;
public class CreateBidCommandValidator : BaseValidator<CreateBidCommand>
{
    public CreateBidCommandValidator()
    {
        RuleFor(x => x.LotId)
            .NotEmpty()
            .WithMessage("Invalid lot");

        RuleFor(x => x.Amount)
            .NotEmpty()
            .WithMessage("Bid amount must be greater than zero");
    }
}
