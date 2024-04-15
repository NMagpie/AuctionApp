using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Infrastructure.Configurations;
public class AuctionReviewConfiguration : IEntityTypeConfiguration<AuctionReview>
{
    public void Configure(EntityTypeBuilder<AuctionReview> builder)
    {
        builder
            .Property(x => x.ReviewText)
            .HasMaxLength(2048);
    }
}