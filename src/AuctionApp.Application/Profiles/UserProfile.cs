using Application.App.Users.Commands;
using Application.App.Users.Responses;
using AutoMapper;
using Domain.Auth;

namespace Application.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();

        CreateMap<User, CurrentUserDto>();

        CreateMap<UpdateUserCommand, User>();
    }
}
