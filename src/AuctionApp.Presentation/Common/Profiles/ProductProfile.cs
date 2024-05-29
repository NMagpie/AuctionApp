using Application.App.Products.Commands;
using AutoMapper;
using Presentation.Common.Models.Products;
using Presentation.Common.Requests.Products;

namespace Presentation.Common.Profiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductRequest, CreateProductCommand>();

        CreateMap<UpdateProductRequest, UpdateProductCommand>();
    }
}
