using AuctionApp.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EntityFramework.Infrastructure.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(x => x.Username)
            .HasMaxLength(64);

        builder
            .HasIndex(x => x.Username)
            .IsUnique();

        builder
            .Property(x => x.Balance)
            .HasColumnType("money")
            .HasDefaultValue(0m);
    }
}
