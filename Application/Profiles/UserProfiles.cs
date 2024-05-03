using Application.App.Users.Commands;
using Application.App.Users.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class UserProfiles : Profile
{
    public UserProfiles()
    {
        CreateMap<User, UserDto>();

        CreateMap<CreateUserCommand, User>();

        CreateMap<UpdateUserCommand, User>();
    }
}
