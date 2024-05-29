using AuctionApp.Domain.Models;
using EntityFramework.Domain.Abstractions;

namespace EntityFramework.Domain.Models;
public class Category : Entity
{
    public string Name { get; set; }

    public ICollection<Product>? Lots { get; set; } = [];
}