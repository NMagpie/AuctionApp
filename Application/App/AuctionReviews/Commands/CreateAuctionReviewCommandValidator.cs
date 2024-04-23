using FluentValidation;

namespace Application.App.AuctionReviews.Commands;
public class CreateAuctionReviewCommandValidator : AbstractValidator<CreateAuctionReviewCommand>
{
    public CreateAuctionReviewCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.AuctionId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.ReviewText)
            .MaximumLength(2048)
            .WithMessage("Review text cannot exceed 2048 characters");

        RuleFor(x => x.Rating)
            .GreaterThan(0)
            .LessThan(10)
            .WithMessage("Rating cannot be less than 0 or greater than 10");
    }
}
