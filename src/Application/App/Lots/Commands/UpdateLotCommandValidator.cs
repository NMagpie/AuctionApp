using Application.Common.Validation;
using FluentValidation;

namespace Application.App.Lots.Commands;
public class UpdateLotCommandValidator : BaseValidator<UpdateLotCommand>
{
    public UpdateLotCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .WithMessage("Invalid id");

        RuleFor(x => x.Title)
            .NotEmpty();

        RuleFor(x => x.Title)
            .MaximumLength(256);

        RuleFor(x => x.Title)
            .MaximumLength(2048);

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0);
    }
}
