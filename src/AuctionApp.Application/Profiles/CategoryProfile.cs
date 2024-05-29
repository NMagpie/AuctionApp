using Application.App.Responses;
using AutoMapper;
using EntityFramework.Domain.Models;

namespace Application.Profiles;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}
