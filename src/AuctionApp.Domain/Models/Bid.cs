using AuctionApp.Domain.Abstractions;
using Domain.Auth;

namespace AuctionApp.Domain.Models;
public class Bid : IEntity
{
    public int Id { get; set; }

    public int ProductId { get; set; }

    public Product? Product { get; set; }

    public int UserId { get; set; }

    public User? User { get; set; }

    public decimal Amount { get; set; }

    public DateTimeOffset CreateTime { get; set; }

    public bool IsWon { get; set; } = false;
}