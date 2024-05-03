﻿using Application.App.UserWatchlists.Commands;
using Application.App.UserWatchlists.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class UserWatchlistProfiles : Profile
{
    public UserWatchlistProfiles()
    {
        CreateMap<UserWatchlist, UserWatchlistDto>();

        CreateMap<CreateUserWatchlistCommand, UserWatchlist>();
    }
}
