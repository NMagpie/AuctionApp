using Application.App.ProductReviews.Commands;
using AutoMapper;
using Presentation.Common.Models.ProductReviews;
using Presentation.Common.Requests.ProductReviews;

namespace Presentation.Common.Profiles;
public class ProductReviewProfile : Profile
{
    public ProductReviewProfile()
    {
        CreateMap<CreateProductReviewRequest, CreateProductReviewCommand>();

        CreateMap<UpdateProductReviewRequest, UpdateProductReviewCommand>();
    }
}
