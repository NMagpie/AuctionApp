using Application.App.AuctionReviews.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using FluentValidation;
using MediatR;

namespace Application.App.AuctionReviews.Commands;

public class UpdateAuctionReviewCommand : IRequest<AuctionReviewDto>
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string ReviewText { get; set; }

    public float Rating { get; set; }
}

public class UpdateAuctionReviewCommandHandler : IRequestHandler<UpdateAuctionReviewCommand, AuctionReviewDto>
{
    private readonly IEntityRepository _repository;

    private readonly UpdateAuctionReviewCommandValidator _validator;

    private readonly IMapper _mapper;

    public UpdateAuctionReviewCommandHandler(IEntityRepository repository, IMapper mapper)
    {
        _repository = repository;
        _validator = new UpdateAuctionReviewCommandValidator();
        _mapper = mapper;
    }

    public async Task<AuctionReviewDto> Handle(UpdateAuctionReviewCommand request, CancellationToken cancellationToken)
    {
        _validator.ValidateAndThrow(request);

        var auctionReview = await _repository.GetById<AuctionReview>(request.Id)
            ?? throw new EntityNotFoundException("AuctionReview cannot be found");

        if (auctionReview.UserId != request.UserId)
        {
            throw new InvalidUserException("You do not have permission to modify this data");
        }

        _mapper.Map(request, auctionReview);

        await _repository.SaveChanges();

        var auctionReviewDto = _mapper.Map<AuctionReview, AuctionReviewDto>(auctionReview);

        return auctionReviewDto;
    }
}
