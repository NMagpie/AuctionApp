using AuctionApp.Application.Common.Abstractions;

namespace AuctionApp.Application.Common.Models;
public class PagedRequest : IPagedRequest
{
    public int PageIndex { get; set; }

    public int PageSize { get; set; }

    public string ColumnNameForSorting { get; set; }

    public string SortDirection { get; set; }

    public string Filter { get; set; }
}
