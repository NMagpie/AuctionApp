using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Infrastructure.Configurations;
public class BidConfiguration : IEntityTypeConfiguration<Bid>
{
    public void Configure(EntityTypeBuilder<Bid> builder)
    {
        builder
            .Property(x => x.Amount)
            .HasColumnType("money");

        builder
            .Property(x => x.IsWon)
            .HasDefaultValue(false);
    }
}
