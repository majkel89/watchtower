using NGitLab;
using NGitLab.Models;
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
            Sort = "full_path",
            MinAccessLevel = AccessLevel.Reporter,
        };
        await foreach (var group in client.Groups.GetAsync(groupQuery))
        {
            var groupProjectsQuery = new GroupProjectsQuery
            {
                Archived = false,
                IncludeSubGroups = true,
                MinAccessLevel = AccessLevel.Reporter,
            };
            await foreach (var project in client.Groups.GetProjectsAsync(group.Id, groupProjectsQuery))
            {
                yield return new Project
                {
                    Source = "GitLab",
                    Id = project.Id.ToString(),
                    Name = project.Name,
                    DefaultBranch = project.DefaultBranch,
                    Namespace = project.Namespace.FullPath,
                    GitUrl = project.SshUrl,
                };
            }
        }
    }
}
