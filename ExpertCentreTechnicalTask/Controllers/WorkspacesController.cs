using ExpertCentreTechnicalTask.Models;
using ExpertCentreTechnicalTask.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExpertCentreTechnicalTask.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkspacesController : ControllerBase
{
    private readonly WorkspaceService _workspaceService;

    public WorkspacesController(WorkspaceService workspaceService)
    {
        _workspaceService = workspaceService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Workspace>> GetAllWorkspaces()
    {
        var workspaces = _workspaceService.GetAllWorkspaces();
        return Ok(workspaces);
    }

    [HttpPost]
    public ActionResult<Workspace> CreateWorkspace([FromBody] Workspace workspace)
    {
        var createdWorkspace = _workspaceService.CreateWorkspace(workspace.Name);
        return CreatedAtAction(nameof(GetWorkspaceById), new { id = createdWorkspace.Id }, createdWorkspace);
    }

    [HttpGet("{id}")]
    public ActionResult<Workspace> GetWorkspaceById(int id)
    {
        var workspace = _workspaceService.GetWorkspaceById(id);
        if (workspace == null)
        {
            return BadRequest(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }
        return Ok(workspace);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateWorkspace(int id, [FromBody] Workspace workspace)
    {
        if (!_workspaceService.UpdateWorkspace(id, workspace.Name))
        {
            return BadRequest(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }
        workspace.Id = id; // Убедитесь, что идентификатор сохраняется
        return Ok(workspace);
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteWorkspace(int id)
    {
        if (!_workspaceService.DeleteWorkspace(id))
        {
            return BadRequest(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
        }
        return Ok(new { id });
    }
}

//[ApiController]
//[Route("api/[controller]")]
//public class WorkspacesController : ControllerBase
//{
//    private readonly WorkspaceService _workspaceService;

//    public WorkspacesController(WorkspaceService workspaceService)
//    {
//        _workspaceService = workspaceService;
//    }

//    [HttpGet]
//    public ActionResult<IEnumerable<Workspace>> GetAllWorkspaces()
//    {
//        var workspaces = _workspaceService.GetAllWorkspaces();
//        return Ok(workspaces);
//    }

//    [HttpPost]
//    public ActionResult<Workspace> CreateWorkspace([FromBody] Workspace workspace)
//    {
//        var createdWorkspace = _workspaceService.CreateWorkspace(workspace.Name);
//        return CreatedAtAction(nameof(GetWorkspaceById), new { id = createdWorkspace.Id }, createdWorkspace);
//    }

//    [HttpGet("{id}")]
//    public ActionResult<Workspace> GetWorkspaceById(int id)
//    {
//        var workspace = _workspaceService.GetWorkspaceById(id);
//        if (workspace == null)
//        {
//            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
//        }
//        return Ok(workspace);
//    }

//    [HttpDelete("{id}")]
//    public IActionResult DeleteWorkspace(int id)
//    {
//        if (!_workspaceService.DeleteWorkspace(id))
//        {
//            return NotFound(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
//        }
//        return Ok(new { id });
//    }

//    [HttpPut("{id}")]
//    public IActionResult UpdateWorkspace(int id, [FromBody] Workspace workspace)
//    {
//        if (!_workspaceService.UpdateWorkspace(id, workspace.Name))
//        {
//            return BadRequest(new { globalErrors = new[] { new { message = "Указанное рабочее пространство не существует" } } });
//        }
//        workspace.Id = id; // Убедитесь, что идентификатор сохраняется
//        return Ok(workspace);
//    }
//}