using Application.App.Lots.Commands;
using Application.App.Lots.Responses;
using Application.App.Responses;
using Application.Common.Models;
using AuctionApp.Domain.Models;
using AutoMapper;
using EntityFramework.Domain.Models;

namespace Application.Profiles;
public class LotProfile : Profile
{
    public LotProfile()
    {
        CreateMap<Lot, LotDto>()
            .ForMember(dest => dest.BidIds, src => src.MapFrom(src => src.Bids.Select(lot => lot.Id)));

        CreateMap<CreateLotCommand, Lot>()
            .ForMember(dest => dest.Categories,
                src => src.MapFrom(
                    src => src.Categories.Select(category => new Category() { Name = category.ToLower() })
                )
            );

        CreateMap<UpdateLotCommand, Lot>()
            .ForMember(dest => dest.Categories,
                src => src.MapFrom(
                    src => src.Categories.Select(category => new Category() { Name = category.ToLower() })
                )
            );

        CreateMap<LotInAuctionDto, Lot>();
    }
}
