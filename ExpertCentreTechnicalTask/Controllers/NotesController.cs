using ExpertCentreTechnicalTask.Models;
using ExpertCentreTechnicalTask.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/workspaces/{workspaceId}/notes")]
public class NotesController : ControllerBase
{
    private readonly NoteService _noteService;
    private readonly WorkspaceService _workspaceService;

    public NotesController(NoteService noteService, WorkspaceService workspaceService)
    {
        _noteService = noteService;
        _workspaceService = workspaceService;
    }

    [HttpGet]
    public ActionResult<object> GetNotesByWorkspace(int workspaceId)
    {
        // Проверка существования рабочего пространства
        var workspace = _workspaceService.GetWorkspaceById(workspaceId);
        if (workspace == null)
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }

        var notes = _noteService.GetNotesByWorkspace(workspaceId);
        return Ok(new { workspaceId = workspaceId, notes = notes.Select(n => new { n.Id, n.Title }) });
    }

    [HttpPost]
    public ActionResult<Note> CreateNoteForWorkspace(int workspaceId)
    {
        var workspace = _workspaceService.GetWorkspaceById(workspaceId);
        if (workspace == null)
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }

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
        var workspace = _workspaceService.GetWorkspaceById(workspaceId);
        if (workspace == null)
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }

        if (!_noteService.UpdateNote(workspaceId, noteId, updatedNote))
        {
            return BadRequest(new { globalErrors = new[] { new { message = "Указанная заметка не существует" } } });
        }
        updatedNote.Id = noteId;
        return Ok(updatedNote);
    }

    [HttpDelete("{noteId}")]
    public IActionResult DeleteNoteById(int workspaceId, int noteId)
    {
        var workspace = _workspaceService.GetWorkspaceById(workspaceId);
        if (workspace == null)
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }

        if (!_noteService.DeleteNoteById(workspaceId, noteId))
        {
            return NotFound(new { globalErrors = new[] { new { message = "Указанная заметка не существует" } } });
        }
        return Ok(new { id = noteId });
    }
}
