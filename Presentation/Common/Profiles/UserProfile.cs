using Application.App.Users.Commands;
using AutoMapper;
using Presentation.Common.Models.Users;

namespace Presentation.Common.Profiles;
public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<UpdateUserRequest, UpdateUserCommand>();

        CreateMap<AddUserBalanceRequest, AddUserBalanceCommand>();
    }
}
