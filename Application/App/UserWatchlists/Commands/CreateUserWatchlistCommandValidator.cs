﻿using FluentValidation;

namespace Application.App.UserWatchlists.Commands;
public class CreateUserWatchlistCommandValidator : AbstractValidator<CreateUserWatchlistCommand>
{
    public CreateUserWatchlistCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.AuctionId)
            .NotEmpty()
            .WithMessage("Invalid auction");
    }
}