using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Auctions.Commands;
public class UpdateAuctionCommandValidator : BaseValidator<UpdateAuctionCommand>
{
    public UpdateAuctionCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.Title)
            .MaximumLength(256);

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTimeOffset.UtcNow + TimeSpan.FromMinutes(5))
            .WithMessage("Start Time must be greater than current time for at least 5 minutes");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage("End Time must be greater than start time");
    }
}
