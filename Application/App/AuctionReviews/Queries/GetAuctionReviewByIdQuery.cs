using Application.Abstractions;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Queries;
public class GetAuctionReviewByIdQuery : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }
}

public class GetAuctionReviewByIdQueryHandler : IRequestHandler<GetAuctionReviewByIdQuery, AuctionReviewDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public GetAuctionReviewByIdQueryHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuctionReviewDto> Handle(GetAuctionReviewByIdQuery request, CancellationToken cancellationToken)
    {
        var auctionReview = await _repository.GetById<AuctionReview>(request.Id)
            ?? throw new ArgumentNullException("Auction Review cannot be found");

        var auctionReviewDto = _mapper.Map<AuctionReview, AuctionReviewDto>(auctionReview);

        return auctionReviewDto;
    }
}