using Application.App.AuctionReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class CreateAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int UserId { get; set; }

    public int AuctionId { get; set; }

    public string? ReviewText { get; set; }

    public float Rating { get; set; }
}

public class CreateAuctionReviewCommandHandler : IRequestHandler<CreateAuctionReviewCommand, AuctionReviewDto>
{

    private readonly IEntityRepository _entityRepository;

    private readonly IUserRepository _userRepository;

    private readonly IMapper _mapper;

    public CreateAuctionReviewCommandHandler(IEntityRepository entityRepository, IUserRepository userRepository, IMapper mapper)
    {
        _entityRepository = entityRepository;
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<AuctionReviewDto> Handle(CreateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetById(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var auction = await _entityRepository.GetById<Auction>(request.AuctionId)
            ?? throw new EntityNotFoundException("Auction cannot be found");

        if (auction.EndTime >= DateTime.UtcNow)
        {
            throw new BusinessValidationException("Cannot put review: auction is not finished");
        }

        var auctionReview = _mapper.Map<CreateAuctionReviewCommand, AuctionReview>(request);

        await _entityRepository.Add(auctionReview);

        await _entityRepository.SaveChanges();

        var auctionReviewDto = _mapper.Map<AuctionReview, AuctionReviewDto>(auctionReview);

        return auctionReviewDto;
    }
}
