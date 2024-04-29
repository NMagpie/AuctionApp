using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
public class UserWatchlistConfiguration : IEntityTypeConfiguration<UserWatchlist>
{
    public void Configure(EntityTypeBuilder<UserWatchlist> builder)
    {
        builder
            .HasIndex(x => new { x.UserId, x.AuctionId })
            .IsUnique();

        builder.Navigation(x => x.User).AutoInclude();

        builder.Navigation(x => x.Auction).AutoInclude();
    }
}
