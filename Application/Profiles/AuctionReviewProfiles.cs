﻿using Application.App.AuctionReviews.Commands;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class AuctionReviewProfiles : Profile
{
    public AuctionReviewProfiles()
    {
        CreateMap<AuctionReview, AuctionReviewDto>();

        CreateMap<CreateAuctionReviewCommand, Auction>();

        CreateMap<UpdateAuctionReviewCommand, Auction>();
    }
}

