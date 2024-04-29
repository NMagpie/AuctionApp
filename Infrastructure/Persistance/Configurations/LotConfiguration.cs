using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
public class LotConfiguration : IEntityTypeConfiguration<Lot>
{
    public void Configure(EntityTypeBuilder<Lot> builder)
    {
        builder
            .Property(x => x.Title)
            .HasMaxLength(256);

        builder
            .Property(x => x.Description)
            .HasMaxLength(2048);

        builder
            .Property(x => x.InitialPrice)
            .HasColumnType("money");

        builder.Navigation(x => x.Auction).AutoInclude();

        builder.Navigation(x => x.Bids).AutoInclude();

        builder.Navigation(x => x.Categories).AutoInclude();
    }
}
