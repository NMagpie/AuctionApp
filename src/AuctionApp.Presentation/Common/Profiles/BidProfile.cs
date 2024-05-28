using Application.App.Bids.Commands;
using AuctionApp.Presentation.Common.Requests.Bids;
using AutoMapper;

namespace AuctionApp.Presentation.Common.Profiles;
public class BidProfile : Profile
{
    public BidProfile()
    {
        CreateMap<CreateBidRequest, CreateBidCommand>();
    }
}
