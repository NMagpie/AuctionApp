using AuctionApp.Domain.Abstractions;
using Domain.Auth;

namespace AuctionApp.Domain.Models;

public class SearchRecord : IEntity
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public User User { get; set; }

    public string SearchQuery { get; set; }

    public DateTimeOffset LastUserAt { get; set; }
}
