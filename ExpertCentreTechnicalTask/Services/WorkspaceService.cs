using ExpertCentreTechnicalTask.Models;

namespace ExpertCentreTechnicalTask.Services;

public class WorkspaceService
{
    private List<Workspace> workspaces = new List<Workspace>();
    private int nextId = 1;

    public WorkspaceService()
    {
        var workspace1 = new Workspace { Name = "Work" };
        var workspace2 = new Workspace { Name = "Test" };
        this.CreateWorkspace(workspace1.Name);
        this.CreateWorkspace(workspace2.Name);
    }

    public Workspace CreateWorkspace(string name)
    {
        var workspace = new Workspace { Id = nextId++, Name = name };
        workspaces.Add(workspace);
        return workspace;
    }

    public IEnumerable<Workspace> GetAllWorkspaces()
    {
        return workspaces;
    }

    public Workspace GetWorkspaceById(int id)
    {
        return workspaces.FirstOrDefault(w => w.Id == id);
    }

    public bool UpdateWorkspace(int id, string newName)
    {
        var workspace = GetWorkspaceById(id);
        if (workspace == null) return false;
        workspace.Name = newName;
        return true;
    }

    public bool DeleteWorkspace(int id)
    {
        var workspace = GetWorkspaceById(id);
        if (workspace == null) return false;
        workspaces.Remove(workspace);
        return true;
    }
}