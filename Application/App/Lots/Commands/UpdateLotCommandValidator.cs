﻿using Application.Common.Validation;
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
            .NotEmpty()
            .WithMessage("Title must be present");

        RuleFor(x => x.Title)
            .MaximumLength(256)
            .WithMessage("Title must be at most 256 characters long");

        RuleFor(x => x.Title)
            .MaximumLength(2048)
            .WithMessage("Description must be at most 2048 characters long");

        RuleFor(x => x.InitialPrice)
            .GreaterThan(0)
            .WithMessage("Initial price must be greater than 0");
    }
}
