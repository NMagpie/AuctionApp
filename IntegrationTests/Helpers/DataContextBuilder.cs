using AuctionApp.Domain.Models;
using Domain.Auth;
using EntityFramework.Domain.Models;
using Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace IntegrationTests.Helpers;
public class DataContextBuilder : IDisposable
{
    private readonly AuctionAppDbContext _dbContext;

    public DataContextBuilder(string dbName = "TestDatabase")
    {
        var options = new DbContextOptionsBuilder<AuctionAppDbContext>()
            .UseInMemoryDatabase(dbName, new InMemoryDatabaseRoot())
            .Options;

        var context = new AuctionAppDbContext(options);

        _dbContext = context;
    }

    public AuctionAppDbContext GetContext()
    {
        _dbContext.Database.EnsureCreated();
        return _dbContext;
    }

    public void SeedAuctions(int number = 1)
    {
        var rnd = new Random();

        var auctions = new List<Auction>();

        var users = _dbContext.Users.ToList();

        if (users.Count == 0)
            return;

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var (
                coef,
                auctionMinStartTime,
                auctionMaxStartTime,
                auctionMinEndTime,
                auctionMaxEndTime
                ) = id % 2 == 0 ? (-1, 120, 180, 30, 110) : (1, 10, 60, 61, 120);

            var auction = new Auction
            {
                Id = id,
                Title = $"Auction-{id}",
                CreatorId = users[rnd.Next(users.Count)].Id,

                StartTime = DateTimeOffset.UtcNow +
                    coef * TimeSpan.FromMinutes(rnd.Next(auctionMinStartTime, auctionMaxStartTime)),

                EndTime = DateTimeOffset.UtcNow +
                    coef * TimeSpan.FromMinutes(rnd.Next(auctionMinEndTime, auctionMaxEndTime))
            };

            auctions.Add(auction);
        }

        _dbContext.AddRange(auctions);
        _dbContext.SaveChanges();
    }

    public void SeedAuctionReviews(int number = 1)
    {
        var rnd = new Random();

        var auctionReviews = new List<AuctionReview>();

        var users = _dbContext.Users.ToList();

        var auctions = _dbContext.Auctions
            .Where(auction => auction.Id % 2 == 0)
            .ToList();

        if (users.Count == 0 || auctions.Count == 0)
            return;

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var auctionReview = new AuctionReview
            {
                Id = id,
                UserId = users[rnd.Next(users.Count)].Id,
                AuctionId = auctions[rnd.Next(auctions.Count)].Id,
                ReviewText = $"review-{id}",
                Rating = rnd.NextSingle() * 9 + 1,
            };

            auctionReviews.Add(auctionReview);
        }

        _dbContext.AddRange(auctionReviews);
        _dbContext.SaveChanges();
    }

    public void SeedBids(int number = 1)
    {
        var rnd = new Random();

        var bids = new List<Bid>();

        var users = _dbContext.Users.ToList();

        var lots = _dbContext.Lots
            .Where(lot => lot.AuctionId % 2 == 0)
            .ToList();

        if (users.Count == 0 || lots.Count == 0)
            return;

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var bid = new Bid
            {
                Id = id,
                LotId = lots[rnd.Next(lots.Count)].Id,
                UserId = users[rnd.Next(users.Count)].Id,
                Amount = new decimal(rnd.NextDouble() * 100),
                CreateTime = DateTimeOffset.UtcNow,
            };

            bids.Add(bid);
        }

        _dbContext.AddRange(bids);
        _dbContext.SaveChanges();
    }

    public void SeedCategories(int number = 1)
    {
        var categories = new List<Category>();

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var category = new Category
            {
                Id = id,
                Name = $"Category-{id}"
            };

            categories.Add(category);
        }

        _dbContext.AddRange(categories);
        _dbContext.SaveChanges();
    }

    public void SeedLots(int number = 1)
    {
        var rnd = new Random();

        var lots = new List<Lot>();

        var auctions = _dbContext.Auctions.ToList();

        var categories = _dbContext.Categories.ToList();

        if (auctions.Count == 0)
            return;

        var selectedCategories = categories.Count == 0 ? [] : new List<Category>() { categories[rnd.Next(categories.Count)] };

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var lot = new Lot
            {
                Id = id,
                Title = $"Lot-{id}",
                Description = $"Desc-Lot-{id}",
                AuctionId = auctions[rnd.Next(auctions.Count)].Id,
                InitialPrice = new decimal(rnd.NextDouble()),
                Categories = selectedCategories,
            };

            lots.Add(lot);
        }

        _dbContext.AddRange(lots);
        _dbContext.SaveChanges();
    }

    public void SeedUsers(int number = 1)
    {
        var rnd = new Random();

        var users = new List<User>();

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var user = new User
            {
                Id = id,
                UserName = $"User-{id}",
                Balance = new decimal(rnd.NextDouble() * 100),
            };

            users.Add(user);
        }

        _dbContext.AddRange(users);
        _dbContext.SaveChanges();
    }

    public void SeedUserWatchlists(int number = 1)
    {
        var rnd = new Random();

        var userWatchlists = new List<UserWatchlist>();

        var auctions = _dbContext.Auctions.ToList();

        var users = _dbContext.Users.ToList();

        if (auctions.Count == 0 || users.Count == 0)
            return;

        for (int i = 0; i < number; i++)
        {
            var id = i + 1;

            var userWatchlist = new UserWatchlist
            {
                Id = id,
                UserId = users[rnd.Next(users.Count)].Id,
                AuctionId = auctions[rnd.Next(auctions.Count)].Id,
            };

            userWatchlists.Add(userWatchlist);
        }

        _dbContext.AddRange(userWatchlists);
        _dbContext.SaveChanges();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }
    }
}
