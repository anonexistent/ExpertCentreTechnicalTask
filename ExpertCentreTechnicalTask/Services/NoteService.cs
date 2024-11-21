using ExpertCentreTechnicalTask.Models;
using System.Collections.Generic;
using System.Linq;

public class NoteService
{
    private readonly Dictionary<int, List<Note>> workspaceNotes = new();
    private int nextNoteId = 4;

    public NoteService()
    {
        // Инициализация первых трех "пробных" записей
        workspaceNotes[1] = new List<Note>
            {
                new Note { Id = 1, Title = "Планирование", Body = new NoteBody() },
                new Note { Id = 2, Title = "Задачи", Body = new NoteBody() },
                new Note { Id = 3, Title = "Отчеты", Body = new NoteBody() }
            };
    }

    public Note CreateNoteForWorkspace(int workspaceId)
    {
        if (!workspaceNotes.ContainsKey(workspaceId))
        {
            workspaceNotes[workspaceId] = new List<Note>();
        }

        var note = new Note
        {
            Id = nextNoteId++,
            Title = "",
            Body = new NoteBody
            {
                Lines = new List<NoteLine>
                    {
                        new NoteLine { Id = 1, Type = 1, Text = "", Styles = new List<string>(), Color = 0 }
                    }
            }
        };

        workspaceNotes[workspaceId].Add(note);
        return note;
    }

    public IEnumerable<Note> GetNotesByWorkspace(int workspaceId)
    {
        return workspaceNotes.ContainsKey(workspaceId) ? workspaceNotes[workspaceId] : Enumerable.Empty<Note>();
    }

    public Note GetNoteById(int workspaceId, int noteId)
    {
        return workspaceNotes.ContainsKey(workspaceId)
            ? workspaceNotes[workspaceId].FirstOrDefault(n => n.Id == noteId)
            : null;
    }

    public bool UpdateNote(int workspaceId, int noteId, Note updatedNote)
    {
        var note = GetNoteById(workspaceId, noteId);
        if (note == null) return false;

        note.Title = updatedNote.Title;
        note.Body = updatedNote.Body;
        return true;
    }

    public bool DeleteNoteById(int workspaceId, int noteId)
    {
        var note = GetNoteById(workspaceId, noteId);
        if (note == null) return false;

        return workspaceNotes[workspaceId].Remove(note);
    }

    public void ResetNoteId()
    {
        nextNoteId = 1;
    }
}