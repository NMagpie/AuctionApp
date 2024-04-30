using AuctionApp.Domain.Models;
using EntityFramework.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;
public class AuctionAppDbContext : DbContext
{

    public AuctionAppDbContext(DbContextOptions<AuctionAppDbContext> options) : base(options)
    {

    }

    public DbSet<Auction> Auctions { get; set; } = default!;

    public DbSet<AuctionReview> AuctionReviews { get; set; } = default!;

    public DbSet<Bid> Bids { get; set; } = default!;

    public DbSet<Category> Categories { get; set; } = default!;

    public DbSet<Lot> Lots { get; set; } = default!;

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<UserWatchlist> UserWatchlists { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionAppDbContext).Assembly);
    }
}