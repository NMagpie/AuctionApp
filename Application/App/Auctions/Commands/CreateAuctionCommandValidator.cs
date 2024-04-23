using FluentValidation;

namespace Application.App.Auctions.Commands;
public class CreateAuctionCommandValidator : AbstractValidator<CreateAuctionCommand>
{
    public CreateAuctionCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title must be present");

        RuleFor(x => x.Title)
            .MaximumLength(256)
            .WithMessage("Title must be at most 256 characters long");

        RuleFor(x => x.CreatorId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Start Time must be greater than current time");

        RuleFor(x => x.EndTime)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("End Time must be greater than current time");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End Time must be greater than start time");

        RuleFor(x => x.Lots.Count)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Must be at least one lot");
    }
}
