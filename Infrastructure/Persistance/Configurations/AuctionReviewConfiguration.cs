using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
public class AuctionReviewConfiguration : IEntityTypeConfiguration<AuctionReview>
{
    public void Configure(EntityTypeBuilder<AuctionReview> builder)
    {
        builder
            .Property(x => x.ReviewText)
            .HasMaxLength(2048);

        builder.Navigation(x => x.User).AutoInclude();

        builder.Navigation(x => x.Auction).AutoInclude();
    }
}