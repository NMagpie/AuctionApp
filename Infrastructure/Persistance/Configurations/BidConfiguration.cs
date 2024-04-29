using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
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

        builder.Navigation(x => x.Lot).AutoInclude();

        builder.Navigation(x => x.User).AutoInclude();
    }
}
