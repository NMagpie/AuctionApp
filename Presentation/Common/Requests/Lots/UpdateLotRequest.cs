namespace Presentation.Common.Models.Lots;
public class UpdateLotRequest
{
    public string Title { get; set; }

    public string Description { get; set; }

    public decimal InitialPrice { get; set; }

    public HashSet<string> Categories { get; set; } = [];
}
