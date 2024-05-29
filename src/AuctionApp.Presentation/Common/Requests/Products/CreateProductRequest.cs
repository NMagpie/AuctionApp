using Domain.Auth;

namespace Presentation.Common.Requests.Products;
public class CreateProductRequest
{
    public string Title { get; set; }

    public string? Description { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<string> Categories { get; set; } = [];
}