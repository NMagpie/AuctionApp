using Application.App.Products.Commands;
using Application.App.Products.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using EntityFramework.Domain.Models;

namespace Application.Profiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.BidIds, src => src.MapFrom(src => src.Bids.Select(product => product.Id)));

        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Categories,
                src => src.MapFrom(
                    src => src.Categories.Select(category => new Category() { Name = category.ToLower() })
                )
            );

        CreateMap<UpdateProductCommand, Product>()
            .ForMember(dest => dest.Categories,
                src => src.MapFrom(
                    src => src.Categories.Select(category => new Category() { Name = category.ToLower() })
                )
            );
    }
}
