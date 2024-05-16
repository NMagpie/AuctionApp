using Application.App.Responses;
using Application.Common.Models;
using AutoMapper;
using EntityFramework.Domain.Models;

namespace Application.Profiles;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CategoryInLotDto, Category>();

        CreateMap<Category, CategoryDto>();
    }
}
