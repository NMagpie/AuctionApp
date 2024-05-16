using Application.App.Auctions.Commands;
using Application.App.Auctions.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;

namespace Application.Profiles;
public class AuctionProfile : Profile
{
    public AuctionProfile()
    {
        CreateMap<Auction, AuctionDto>()
            .ForMember(dest => dest.LotIds, src => src.MapFrom(dest => dest.Lots.Select(lot => lot.Id)));

        CreateMap<CreateAuctionCommand, Auction>()
            .ForMember(dest => dest.Lots, src => src.MapFrom(dest => dest.Lots));

        CreateMap<UpdateAuctionCommand, Auction>();
    }
}
