using Application.App.ProductReviews.Commands;
using Application.Common.Validation;
using FluentValidation;

namespace Application.App.AuctionReviews.Commands;
public class CreateProductReviewCommandValidator : BaseValidator<CreateProductReviewCommand>
{
    public CreateProductReviewCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("Invalid user");

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Invalid product");

        RuleFor(x => x.ReviewText)
            .MaximumLength(2048);

        RuleFor(x => x.Rating)
            .GreaterThan(0)
            .LessThan(5);
    }
}
