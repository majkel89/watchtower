using Watchtower.Core;

namespace Watchtower.Playground;

public class Storage
{
    private string Root { get; }

    public Storage(string root)
    {
        Root = root;
        Directory.CreateDirectory(root);
    }

    public async Task PutFileAsync(Project project, ProjectFile file, Task<string> contents)
    {
        var dir = $"{GetProjectPath(project)}/{file.DirPath}";
        Directory.CreateDirectory(dir);
        await File.WriteAllTextAsync($"{dir}/{file.FileName}", await contents);
    }

    public bool IsProjectStoredAsync(Project project) => Directory.Exists(GetProjectPath(project));

    private string GetProjectPath(Project project) =>
        $"{Root}/@{project.Namespace}@/@@{project.Name}@@/__{project.DefaultBranch}__";
}
