using Application.App.ProductReviews.Commands;
using Application.App.ProductReviews.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class ProductReviewProfile : Profile
{
    public ProductReviewProfile()
    {
        CreateMap<ProductReview, ProductReviewDto>();

        CreateMap<CreateProductReviewCommand, ProductReview>();

        CreateMap<UpdateProductReviewCommand, ProductReview>();
    }
}

