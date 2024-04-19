using Application.Abstractions;
using Application.App.Commands.Bids;
using AuctionApp.Domain.Enumerators;
using AuctionApp.Domain.Models;
using System.Timers;

namespace Application.Context;
public class AuctionContext
{
    public Auction Auction { get; init; }

    private readonly IUnitOfWork _unitOfWork;

    private List<Lot> _lots = [];

    public Lot CurrentLot { get; private set; }

    private int _currentLotIndex = 0;

    public TimeSpan LotDuration { get; private set; }

    private readonly object lockCurrentLot = new();

    private readonly System.Timers.Timer startTimer = new();

    private readonly System.Timers.Timer endTimer = new();

    private readonly System.Timers.Timer lotTimer = new();

    public AuctionContext(Auction auction, IUnitOfWork unitOfWork)
    {
        Auction = auction;
        _unitOfWork = unitOfWork;

        startTimer.Elapsed += StartAuction;

        startTimer.Interval = (DateTimeOffset.UtcNow - Auction.StartTime)!.Value.TotalMilliseconds;

        startTimer.Start();
    }

    public DateTimeOffset CurrentLotEndTime => (DateTimeOffset)(Auction.StartTime! + (LotDuration * (_currentLotIndex + 1)));

    public void StartAuction(object? source, ElapsedEventArgs? e)
    {
        startTimer.Stop();

        if (Auction.StartTime is null)
        {
            throw new ArgumentNullException("Start Time cannot be null");
        }

        if (Auction.EndTime is null)
        {
            throw new ArgumentNullException("End Time cannot be null");
        }

        if (Auction.Lots!.Count == 0)
        {
            throw new ArgumentException("There must be at least one lot to start the auction");
        }

        _lots = Auction.Lots
            .OrderBy(lot => lot.LotOrder)
            .ToList();

        LotDuration = (TimeSpan)(Auction.EndTime - Auction.StartTime) / _lots.Count;

        if (LotDuration < TimeSpan.FromSeconds(30) ||
            LotDuration > TimeSpan.FromDays(10))
        {
            throw new ArgumentException("Minimum trade duration for each slot is 30 seconds. Maximum is 10 days.");
        }

        CurrentLot = _lots[0];

        endTimer.Elapsed += FinishAuction;

        endTimer.Interval = (Auction.StartTime - Auction.EndTime).Value.TotalMilliseconds;

        lotTimer.Elapsed += NextLot;

        lotTimer.Interval = LotDuration.TotalMilliseconds;

        endTimer.Start();

        lotTimer.Start();

        Auction.StatusId = (int)AuctionStatusId.Active;

        _unitOfWork.SaveChanges();
    }

    public void FinishAuction(object? source = null, ElapsedEventArgs? e = null)
    {
        Auction.StatusId = (int)AuctionStatusId.Finished;

        _unitOfWork.SaveChanges();

        endTimer.Stop();

        lotTimer.Stop();

        startTimer.Dispose();

        endTimer.Dispose();

        lotTimer.Dispose();
    }

    public void NextLot(object? source, ElapsedEventArgs? e)
    {
        lock (lockCurrentLot)
        {
            var winningBid = CurrentLot.Bids?.Where(
                bid => bid.Amount == CurrentLot.Bids.Max(bid => bid.Amount)
                ).FirstOrDefault();

            if (winningBid != null)
            {
                winningBid.IsWon = true;

                _unitOfWork.SaveChanges();
            }

            CurrentLot = _lots[++_currentLotIndex];

            lotTimer.Stop();
            lotTimer.Start();
        }
    }

    public Bid PlaceBid(CreateBidCommand request)
    {
        if (request.LotId != CurrentLot.Id)
        {
            throw new ArgumentException("Invalid lot");
        }

        if (CurrentLotEndTime <= DateTime.UtcNow || Auction.StatusId != (int)AuctionStatusId.Active)
        {
            throw new ArgumentException("Lot trade time is out");
        }

        var currentMax = CurrentLot.Bids!.Max(bid => bid.Amount);

        if (request.Amount <= currentMax)
        {
            throw new ArgumentException("Bid amount is too low");
        }

        lock (lockCurrentLot)
        {
            if (CurrentLotEndTime <= DateTime.UtcNow || Auction.StatusId != (int)AuctionStatusId.Active)
            {
                throw new ArgumentException("Lot trade time is out");
            }

            currentMax = CurrentLot.Bids!.Max(bid => bid.Amount);

            if (request.Amount <= currentMax)
            {
                throw new ArgumentException("Bid amount is too low");
            }

            var bid = new Bid
            {
                LotId = request.LotId,
                Lot = CurrentLot,
                Amount = request.Amount,
                CreateTime = DateTime.UtcNow,
                IsWon = false,
            };

            CurrentLot.Bids!.Add(bid);

            _unitOfWork.SaveChanges();

            return bid;
        }
    }
}
