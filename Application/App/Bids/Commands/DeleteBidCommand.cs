using Application.App.Bids.Responses;
using Application.Common.Abstractions;
using Application.Common.Exceptions;
using AuctionApp.Domain.Models;
using AutoMapper;
using MediatR;

namespace Application.App.Bids.Commands;

public class DeleteBidCommand : IRequest
{
    public int Id { get; set; }
}


public class DeleteBidCommandHandler : IRequestHandler<DeleteBidCommand>
{

    private readonly IRepository _repository;

    private readonly IMapper _mapper;

    public DeleteBidCommandHandler(IRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task Handle(DeleteBidCommand request, CancellationToken cancellationToken)
    {
        var bid = await _repository.GetById<Bid>(request.Id)
            ?? throw new EntityNotFoundException("Bid cannot be found");

        if (bid.Lot.Auction.StartTime <= DateTime.UtcNow + TimeSpan.FromMinutes(5))
        {
            throw new BusinessValidationException("Cannot edit lots of auction 5 minutes before its start");
        }

        await _repository.Remove<Bid>(request.Id);

        await _repository.SaveChanges();
    }
}
