namespace AuctionApp.Application.Common.Models;

public class PaginatedResult<T>
{
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public IList<T> Items { get; set; }
}
