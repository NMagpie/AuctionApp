namespace Presentation.Common.Models.Products;
public class UpdateProductRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public DateTimeOffset? StartTime { get; set; }

    public DateTimeOffset? EndTime { get; set; }

    public decimal InitialPrice { get; set; }

    public string Category { get; set; }
}
