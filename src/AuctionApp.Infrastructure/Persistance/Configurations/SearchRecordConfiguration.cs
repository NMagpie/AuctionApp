using AuctionApp.Domain.Models;
using Domain.Auth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionApp.Infrastructure.Persistance.Configurations;

public class SearchRecordConfiguration : IEntityTypeConfiguration<SearchRecord>
{
    public void Configure(EntityTypeBuilder<SearchRecord> builder)
    {
        builder
            .Property(x => x.SearchQuery)
            .UseCollation("SQL_Latin1_General_CP1_CS_AS")
            .HasMaxLength(2048);
    }
}
