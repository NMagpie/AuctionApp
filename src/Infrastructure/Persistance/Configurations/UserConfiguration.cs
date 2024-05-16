using Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistance.Configurations;
public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .Property(x => x.UserName)
            .HasMaxLength(64);

        builder
            .HasIndex(x => x.UserName)
            .IsUnique();

        builder
            .Property(x => x.Balance)
            .HasColumnType("money")
            .HasDefaultValue(0m);
    }
}
