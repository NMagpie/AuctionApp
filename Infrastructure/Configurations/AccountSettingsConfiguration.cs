using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations;
public class AccountSettingsConfiguration : IEntityTypeConfiguration<AccountSettings>
{
    public void Configure(EntityTypeBuilder<AccountSettings> builder)
    {
        builder
            .Property(x => x.BillingInfo)
            .HasMaxLength(1024);

        builder
            .Property(x => x.TwoFactorAuthentication)
            .HasMaxLength(1024);
    }
}
