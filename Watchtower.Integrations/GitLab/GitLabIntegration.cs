using System.Net;
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
        var c = new ProjectQuery
        {
            Scope = ProjectQueryScope.Accessible,
            Archived = false,
            Simple = true,
            Ascending = true,
            OrderBy = "path",
            PerPage = 10,
            Search = "group = international"
        };

        await foreach (var project in client.Projects.GetAsync(c))
        {
            var ret = false;
            try
            {
                var treeSearch = new RepositoryGetTreeOptions
                {
                    Recursive = false,
                };
                await foreach (var file in client.GetRepository(project.Id).GetTreeAsync(treeSearch))
                {
                    Console.WriteLine($"\t {file.Path}");
                }
                ret = true;
            }
            catch (GitLabException e)
            {
                if (e.StatusCode == HttpStatusCode.Forbidden)
                    continue;
                throw;
            }

            if (ret)
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
