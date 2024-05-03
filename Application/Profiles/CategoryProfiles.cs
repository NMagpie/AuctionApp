using Application.App.Responses;
using Application.Models;
using AutoMapper;
using EntityFramework.Domain.Models;

namespace Application.Profiles;
public class CategoryProfiles : Profile
{
    public CategoryProfiles()
    {
        CreateMap<CategoryInLotDto, Category>();

        CreateMap<Category, CategoryDto>();
    }
}
