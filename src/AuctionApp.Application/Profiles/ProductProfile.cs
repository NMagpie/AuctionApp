using Application.App.Products.Commands;
using Application.App.Products.Responses;
using AuctionApp.Application.App.Products.Queries;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductCommand, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        CreateMap<UpdateProductCommand, Product>()
            .ForMember(dest => dest.Category, opt => opt.Ignore());

        CreateMap<SearchProductsQuery, PagedRequest>();
    }
}
