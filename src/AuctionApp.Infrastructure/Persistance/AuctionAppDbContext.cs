using AuctionApp.Domain.Models;
using Domain.Auth;
using EntityFramework.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistance;
public class AuctionAppDbContext : IdentityDbContext<User, IdentityRole<int>, int>
{
    public AuctionAppDbContext(DbContextOptions<AuctionAppDbContext> options) : base(options) { }

    public DbSet<ProductReview> ProductReviews { get; set; } = default!;

    public DbSet<Bid> Bids { get; set; } = default!;

    public DbSet<Category> Categories { get; set; } = default!;

    public DbSet<Product> Products { get; set; } = default!;

    public DbSet<UserWatchlist> UserWatchlists { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuctionAppDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}