namespace AuctionApp.Application.Common.Abstractions;

public interface IPagedRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }
}
