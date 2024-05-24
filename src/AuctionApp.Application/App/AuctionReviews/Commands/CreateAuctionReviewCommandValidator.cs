﻿using Application.Common.Validation;
using FluentValidation;

namespace Application.App.AuctionReviews.Commands;
public class CreateAuctionReviewCommandValidator : BaseValidator<CreateAuctionReviewCommand>
{
    public CreateAuctionReviewCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.AuctionId)
            .NotEmpty()
            .WithMessage("Invalid auction");

        RuleFor(x => x.ReviewText)
            .MaximumLength(2048);

        RuleFor(x => x.Rating)
            .GreaterThan(0)
            .LessThan(10);
    }
}