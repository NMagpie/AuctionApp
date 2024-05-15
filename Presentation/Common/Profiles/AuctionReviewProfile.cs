using Application.App.AuctionReviews.Commands;
using AutoMapper;
using Presentation.Common.Models.AuctionReviews;
using Presentation.Common.Requests.AuctionReviews;

namespace Presentation.Common.Profiles;
public class AuctionReviewProfile : Profile
{
    public AuctionReviewProfile()
    {
        CreateMap<CreateAuctionReviewRequest, CreateAuctionReviewCommand>();

        CreateMap<UpdateAuctionReviewRequest, UpdateAuctionReviewCommand>();
    }
}
