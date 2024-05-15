using Application.App.Auctions.Commands;
using AutoMapper;
using Presentation.Common.Models.Auctions;
using Presentation.Common.Requests.Auctions;

namespace Presentation.Common.Profiles;
public class AuctionProfile : Profile
{
    public AuctionProfile()
    {
        CreateMap<CreateAuctionRequest, CreateAuctionCommand>();

        CreateMap<UpdateAuctionRequest, UpdateAuctionCommand>();
    }
}
