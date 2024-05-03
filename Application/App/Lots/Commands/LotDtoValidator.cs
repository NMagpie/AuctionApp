using Application.Models;
using FluentValidation;

namespace Application.App.Lots.Commands;
public class LotDtoValidator : AbstractValidator<LotInAuctionDto>
{
    public LotDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Title must be present");

        RuleFor(x => x.Title)
            .MaximumLength(256)
            .WithMessage("Title must be at most 256 characters long");

        RuleFor(x => x.Description)
            .MaximumLength(2048)
            .WithMessage("Description must be at most 2048 characters long");

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0)
            .WithMessage("Initial price must be greater than 0");
    }
}
