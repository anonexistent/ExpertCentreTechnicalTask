namespace ExpertCentreTechnicalTask.Models;

public class NoteLine
{
    public int Id { get; set; }
    public int Type { get; set; }
    public string Text { get; set; } = string.Empty;
    public List<string> Styles { get; set; } = new List<string>();
    public int Color { get; set; }
}
