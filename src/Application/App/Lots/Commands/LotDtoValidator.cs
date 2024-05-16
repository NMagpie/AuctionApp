using Application.Common.Models;
using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Lots.Commands;
public class LotDtoValidator : BaseValidator<LotInAuctionDto>
{
    public LotDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Title)
            .MaximumLength(256);

        RuleFor(x => x.Description)
            .MaximumLength(2048);

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0);
    }
}
