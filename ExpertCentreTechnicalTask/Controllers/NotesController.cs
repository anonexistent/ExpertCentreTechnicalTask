using ExpertCentreTechnicalTask.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/workspaces/{workspaceId}/notes")]
public class NotesController : ControllerBase
{
    private readonly NoteService _noteService;

    public NotesController(NoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    public ActionResult<object> GetNotesByWorkspace(int workspaceId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return Unauthorized();
        }

        var notes = _noteService.GetNotesByWorkspace(workspaceId)
            .Select(n => new { n.Id, n.Title }); // Исключаем body из ответа

        if (!notes.Any())
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }
        return Ok(new { workspaceId = workspaceId, notes = notes });
    }

    [HttpPost]
    public ActionResult<Note> CreateNoteForWorkspace(int workspaceId)
    {
        var note = _noteService.CreateNoteForWorkspace(workspaceId);
        return CreatedAtAction(nameof(GetNoteById), new { workspaceId, noteId = note.Id }, note);
    }

    [HttpGet("{noteId}")]
    public ActionResult<Note> GetNoteById(int workspaceId, int noteId)
    {
        var note = _noteService.GetNoteById(workspaceId, noteId);
        if (note == null)
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанная заметка не существует" } } });
        }
        return Ok(note);
    }

    [HttpPut("{noteId}")]
    public IActionResult UpdateNote(int workspaceId, int noteId, [FromBody] Note updatedNote)
    {
        if (!_noteService.UpdateNote(workspaceId, noteId, updatedNote))
        {
            return BadRequest(new { globalErrors = new[] { new { message = "Указанная заметка не существует" } } });
        }
        updatedNote.Id = noteId; // Убедитесь, что идентификатор сохраняется
        return Ok(updatedNote);
    }

    [HttpDelete("{noteId}")]
    public IActionResult DeleteNoteById(int workspaceId, int noteId)
    {
        if (!_noteService.DeleteNoteById(workspaceId, noteId))
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанная заметка не существует" } } });
        }
        return Ok(new { id = noteId });
    }
}
