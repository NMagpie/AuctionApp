using Application.App.Lots.Commands;
using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Auctions.Commands;
public class CreateAuctionCommandValidator : BaseValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Title)
            .MaximumLength(256);

        RuleFor(x => x.CreatorId)
            .NotEmpty();

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTimeOffset.UtcNow + TimeSpan.FromMinutes(5))
            .WithMessage("Start Time must be greater than current time for at least 5 minutes");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End Time must be greater than start time");

        RuleFor(x => x.Lots)
            .NotEmpty()
            .WithMessage("Must be at least one lot");

        RuleForEach(x => x.Lots).SetValidator(new LotDtoValidator());
    }
}
