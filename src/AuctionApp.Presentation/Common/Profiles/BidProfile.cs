using Application.App.Bids.Commands;
using AuctionApp.Presentation.SignalR.Dtos;
using AutoMapper;

namespace AuctionApp.Presentation.Common.Profiles;
public class BidProfile : Profile
{
    public BidProfile()
    {
        CreateMap<CreateBidRequest, CreateBidCommand>();
    }
}
