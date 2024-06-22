namespace AuctionApp.Application.App.SearchQueries.Responses;

public class SearchRecordDto
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string SearchQuery { get; set; }

    public DateTimeOffset LastUserAt { get; set; }
}
