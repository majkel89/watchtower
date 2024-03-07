using NGitLab;
using NGitLab.Models;
using Watchtower.Core;
using Project = Watchtower.Core.Project;

namespace Watchtower.Integrations.GitLab;

public sealed class GitLabIntegration(IGitLabClient client)
{
    public static GitLabIntegration Create(string url, string privateToken)
    {
        return new GitLabIntegration(new GitLabClient(url, privateToken));
    }

    public async IAsyncEnumerable<Project> ListProjectsAsync()
    {
        var groupQuery = new GroupQuery
        {
            MinAccessLevel = AccessLevel.Reporter
        };
        await foreach (var group in client.Groups.GetAsync(groupQuery))
        {
            var groupProjectsQuery = new GroupProjectsQuery
            {
                Archived = false,
                IncludeSubGroups = true,
                MinAccessLevel = AccessLevel.Reporter
            };
            await foreach (var project in client.Groups.GetProjectsAsync(group.Id, groupProjectsQuery))
            {
                if (project.EmptyRepo)
                    continue;

                yield return new Project
                {
                    Source = "GitLab",
                    Id = project.Id.ToString(),
                    Name = project.Path,
                    DefaultBranch = project.DefaultBranch,
                    Namespace = project.Namespace.FullPath,
                    GitUrl = project.SshUrl,
                    Files = ScanTreeAsync(project.Id)
                };
            }
        }
    }

    private async IAsyncEnumerable<ProjectFile> ScanTreeAsync(int projectId, string path = "", int depth = 0, int maxDepth = 1)
    {
        var treeOptions = new RepositoryGetTreeOptions
        {
            Path = path,
            Recursive = false
        };
        await foreach (var file in client.GetRepository(projectId).GetTreeAsync(treeOptions))
        {
            switch (file.Type)
            {
                case ObjectType.tree when depth < maxDepth:
                    {
                        await foreach (var innerFile in ScanTreeAsync(projectId, file.Path, depth + 1, maxDepth))
                            yield return innerFile;
                        break;
                    }
                case ObjectType.blob:
                    {
                        yield return new ProjectFile
                        {
                            Path = file.Path
                        };
                        break;
                    }
            }
        }
    }
}
