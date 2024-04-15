using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EntityFramework.Infrastructure;
public class AuctionAppDbContext : DbContext
{
    public DbSet<Auction> Auctions { get; set; } = default!;

    public DbSet<AuctionReview> AuctionReviews { get; set; } = default!;

    public DbSet<Bid> Bids { get; set; } = default!;

    public DbSet<Lot> Lots { get; set; } = default!;

    public DbSet<User> Users { get; set; } = default!;

    public DbSet<UserWatchlist> UserWatchlists { get; set; } = default!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //(localdb)\mssqllocaldb

        optionsBuilder
            .UseSqlServer(@"Server=localhost;Database=AuctionAppEF;Trusted_Connection=True;MultipleActiveResultSets=False;Integrated Security=true;Encrypt=false")
            .LogTo(Console.WriteLine, new[] { DbLoggerCategory.Database.Command.Name },
            LogLevel.Information);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionAppDbContext).Assembly);
    }
}