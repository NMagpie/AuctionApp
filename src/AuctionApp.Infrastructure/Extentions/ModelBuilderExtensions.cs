using EntityFramework.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Infrastructure.Extentions;

public static class ModelBuilderExtensions
{
    public static void Seed(this ModelBuilder modelBuilder)
    {
        var defalutCategories = new string[] {
            "Antiques",
            "Books",
            "Electronics",
            "Fashion",
            "Other"
        };

        modelBuilder.Entity<Category>().HasData(
            defalutCategories.Select((category, index) => new Category { Id = index + 1, Name = category })
        );
    }
}
