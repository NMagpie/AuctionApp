using Domain.Auth;
using EntityFramework.Domain.Abstractions;

namespace AuctionApp.Domain.Models;
public class Bid : Entity
{
    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset CreateTime { get; set; }

    public bool IsWon { get; set; } = false;
}