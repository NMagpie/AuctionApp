using FluentValidation;

namespace Application.App.Commands.Auctions;
public class UpdateAuctionCommandValidator : AbstractValidator<UpdateAuctionCommand>
{
    public UpdateAuctionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title must be present");

        RuleFor(x => x.Title)
            .MaximumLength(256)
            .WithMessage("Title must be at most 256 characters long");

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("Start Time must be greater than current time");

        RuleFor(x => x.EndTime)
            .GreaterThan(DateTimeOffset.UtcNow)
            .WithMessage("End Time must be greater than current time");
    }
}
