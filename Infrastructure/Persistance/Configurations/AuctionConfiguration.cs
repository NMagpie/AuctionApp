using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
{
    public void Configure(EntityTypeBuilder<Auction> builder)
    {
        builder
            .Property(x => x.Title)
            .HasMaxLength(256);

        builder.Navigation(x => x.Creator).AutoInclude();

        builder.Navigation(x => x.Lots).AutoInclude();
    }
}
