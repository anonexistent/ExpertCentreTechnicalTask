namespace ExpertCentreTechnicalTask.Models;

public class Note
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public NoteBody Body { get; set; } = new NoteBody();
}
