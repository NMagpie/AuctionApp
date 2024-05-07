using Application.App.AuctionReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
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

    private readonly IRepository _repository;

    private readonly CreateAuctionReviewCommandValidator _validator;

    private readonly IMapper _mapper;

    public CreateAuctionReviewCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new CreateAuctionReviewCommandValidator();
        _mapper = mapper;
    }

    public async Task<AuctionReviewDto> Handle(CreateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var user = await _repository.GetById<User>(request.UserId)
            ?? throw new EntityNotFoundException("User cannot be found");

        var auction = await _repository.GetById<Auction>(request.AuctionId)
            ?? throw new EntityNotFoundException("Auction cannot be found");

        if (auction.EndTime >= DateTime.UtcNow)
        {
            throw new BusinessValidationException("Cannot put review: auction is not finished");
        }

        var auctionReview = _mapper.Map<CreateAuctionReviewCommand, AuctionReview>(request);

        await _repository.Add(auctionReview);

        await _repository.SaveChanges();

        var auctionReviewDto = _mapper.Map<AuctionReview, AuctionReviewDto>(auctionReview);

        return auctionReviewDto;
    }
}
