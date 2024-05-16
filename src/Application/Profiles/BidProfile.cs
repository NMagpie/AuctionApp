using Application.App.Bids.Commands;
using Application.App.Bids.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class BidProfile : Profile
{
    public BidProfile()
    {
        CreateMap<Bid, BidDto>();

        CreateMap<CreateBidCommand, Bid>();
    }
}
