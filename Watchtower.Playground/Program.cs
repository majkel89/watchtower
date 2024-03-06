using Watchtower.Integrations.GitLab;
using Watchtower.Playground;

DotEnv.Load(".env");

var gitLabUrl = Environment.GetEnvironmentVariable("GITLAB_URL") ?? "https://gitlab.com/";
var gitLabToken = Environment.GetEnvironmentVariable("GITLAB_TOKEN") ?? "***";

var gitlab = GitLabIntegration.Create(gitLabUrl, gitLabToken);

Console.WriteLine($"Listing GitLab projects: {gitLabUrl}");

await foreach (var project in gitlab.ListProjectsAsync())
{
    Console.WriteLine($"{project.Source} {project.Id}: {project.Namespace}/{project.Name} ({project.DefaultBranch})");
}
