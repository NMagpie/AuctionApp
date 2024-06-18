using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Products.Commands;
public class CreateProductCommandValidator : BaseValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MaximumLength(256);

        RuleFor(x => x.Description)
            .MaximumLength(2048);

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0);

        RuleFor(x => x.CreatorId)
            .NotEmpty();

        RuleFor(x => x.StartTime)
            .GreaterThan(DateTimeOffset.UtcNow + TimeSpan.FromMinutes(5))
            .WithMessage("The Start Time must be greater than current time for at least 5 minutes");

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime + TimeSpan.FromMinutes(1))
            .WithMessage("The Selling Time must be at least 1 minute long");
    }
}
