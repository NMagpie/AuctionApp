using AuctionApp.Domain.Abstractions;
using AuctionApp.Domain.Models;

namespace EntityFramework.Domain.Models;
public class Category : IEntity
{
    public int Id { get; set; }

    public string Name { get; set; }

    public ICollection<Product>? Lots { get; set; } = [];
}