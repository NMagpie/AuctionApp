using AuctionApp.Domain.Models;
using Domain.Auth;
using EntityFramework.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AuctionApp.Infrastructure.Extentions;

public static class ModelBuilderSeederExtensions
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

        modelBuilder.DemoDataSeed();
    }

    private static void DemoDataSeed(this ModelBuilder modelBuilder)
    {
        var rand = new Random();

        var users = new List<User>();

        for (int i = 1; i <= 100; i++)
        {
            var user = new User
            {
                Id = i,
                Email = $"user{i}@gmail.com",
                UserName = $"user-{i}",
                Balance = GenerateRandomDecimal(rand, 0, 1500)
            };

            var ph = new PasswordHasher<User>();
            user.PasswordHash = ph.HashPassword(user, "Test123_");

            users.Add(user);
        }

        var products = new List<Product>();

        //finished sellings
        for (int i = 1; i <= 100; i++)
        {
            var product = new Product
            {
                Id = i,
                Title = $"Product-{i}",
                Description = $"Product-Description-{i}",
                CreatorId = rand.Next(1, 100),
                StartTime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(i + 100),
                EndTime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(i + 90),
                SellingFinished = false,
                InitialPrice = GenerateRandomDecimal(rand, 1, 1500),
                CategoryId = rand.Next(1, 6),
            };

            products.Add(product);
        }

        //current sellings
        for (int i = 101; i <= 200; i++)
        {
            var product = new Product
            {
                Id = i,
                Title = $"Product-{i}",
                Description = $"Product-Description-{i}",
                CreatorId = rand.Next(1, 100),
                StartTime = DateTimeOffset.UtcNow - TimeSpan.FromMinutes(i - 90),
                EndTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(i - 100),
                SellingFinished = false,
                InitialPrice = GenerateRandomDecimal(rand, 1, 1500),
                CategoryId = rand.Next(1, 6),
            };

            products.Add(product);
        }

        //future sellings
        for (int i = 201; i <= 300; i++)
        {
            var product = new Product
            {
                Id = i,
                Title = $"Product-{i}",
                Description = $"Product-Description-{i}",
                CreatorId = rand.Next(1, 100),
                StartTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(i - 200),
                EndTime = DateTimeOffset.UtcNow + TimeSpan.FromMinutes(i),
                SellingFinished = false,
                InitialPrice = GenerateRandomDecimal(rand, 1, 1500),
                CategoryId = rand.Next(1, 6),
            };

            products.Add(product);
        }

        var bids = new List<Bid>();

        for (int i = 1; i <= 400; i++)
        {

            var product = products[rand.Next(0, 200)];

            var bid = new Bid
            {
                Id = i,
                ProductId = product.Id,
                UserId = rand.Next(1, 100),
                Amount = GenerateRandomDecimal(rand, product.InitialPrice + 1, 1501),
                CreateTime = GenerateRandomDate(rand, product.StartTime.Value, product.EndTime.Value)
            };

            bids.Add(bid);
        }

        var maxAmountsByProduct = bids
            .GroupBy(b => b.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                MaxAmount = g.Max(b => b.Amount)
            }).ToList();

        foreach (var maxAmount in maxAmountsByProduct)
        {
            bids
                .Where(b => b.ProductId == maxAmount.ProductId && b.Amount == maxAmount.MaxAmount)
                .ToList()
                .ForEach(b => b.IsWon = true);
        }

        var productReviews = new List<ProductReview>();

        for (int i = 1; i <= 300; i++)
        {
            var user = users[rand.Next(0, 100)];

            var product = products[rand.Next(0, 100)];

            var review = new ProductReview
            {
                Id = i,
                UserId = user.Id,
                ProductId = product.Id,
                ReviewText = $"Wow, the another review no. {i} by user {user.UserName}!",
                Rating = (float)(rand.Next(1, 11) * 0.5),
                DateCreated = GenerateRandomDate(rand, product.StartTime.Value, product.EndTime.Value),
            };

            productReviews.Add(review);
        }

        var userWatchlists = new List<UserWatchlist>();

        for (int i = 1; i <= 300; i++)
        {
            var product = products[rand.Next(0, 300)];

            var startTime = product.StartTime.Value > DateTimeOffset.UtcNow ? DateTimeOffset.UtcNow : product.StartTime.Value;

            var userWatchlist = new UserWatchlist
            {
                Id = i,
                UserId = rand.Next(1, 100),
                ProductId = rand.Next(1, 300),
                Created = GenerateRandomDate(rand, startTime, DateTimeOffset.UtcNow)
            };

            userWatchlists.Add(userWatchlist);
        }

        userWatchlists = userWatchlists.DistinctBy(u => new { u.UserId, u.ProductId }).ToList();

        modelBuilder.Entity<User>().HasData(users);

        modelBuilder.Entity<Product>().HasData(products);

        modelBuilder.Entity<Bid>().HasData(bids);

        modelBuilder.Entity<ProductReview>().HasData(productReviews);

        modelBuilder.Entity<UserWatchlist>().HasData(userWatchlists);
    }

    private static decimal GenerateRandomDecimal(Random random, decimal minValue, decimal maxValue)
    {
        double range = (double)(maxValue - minValue);
        double sample = random.NextDouble();
        double scaled = (sample * range) + (double)minValue;

        return decimal.Round((decimal)scaled, 2);
    }

    private static DateTimeOffset GenerateRandomDate(Random random, DateTimeOffset startTime, DateTimeOffset endTime)
    {
        return DateTimeOffset.FromUnixTimeSeconds(
            random.NextInt64(
                startTime.ToUnixTimeSeconds(),
                endTime.ToUnixTimeSeconds()
                ));
    }
}
