using Application.App.Products.Commands;
using Application.App.Products.Responses;
using AuctionApp.Application.App.Products.Queries;
using AuctionApp.Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using EntityFramework.Domain.Models;

namespace Application.Profiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<Product, ProductDto>();

        CreateMap<CreateProductCommand, Product>();

        CreateMap<UpdateProductCommand, Product>();

        CreateMap<SearchProductsQuery, PagedRequest>();
    }
}
