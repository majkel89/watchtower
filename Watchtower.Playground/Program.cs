using Watchtower.Integrations.GitLab;

const string gitLabUrl = "https://gitlab.com/";
const string gitLabToken = "***";

var gitlab = GitLabIntegration.Create(gitLabUrl, gitLabToken);

Console.WriteLine($"Listing GitLab projects: {gitLabUrl}");

await foreach (var project in gitlab.ListProjectsAsync())
{
    Console.WriteLine($"{project.Source} {project.Id}: {project.Namespace}/{project.Name} ({project.DefaultBranch})");
}
