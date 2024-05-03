using Application.Abstractions;
using Application.App.AuctionReviews.Responses;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class DeleteAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }
}

public class DeleteAuctionReviewCommandHandler : IRequestHandler<DeleteAuctionReviewCommand, AuctionReviewDto>
{
    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public DeleteAuctionReviewCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<AuctionReviewDto> Handle(DeleteAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        var auctionReview = await _repository.Remove<AuctionReview>(request.Id);

        await _repository.SaveChanges();

        var auctionReviewDto = _mapper.Map<AuctionReview, AuctionReviewDto>(auctionReview);

        return auctionReviewDto;
    }
}
