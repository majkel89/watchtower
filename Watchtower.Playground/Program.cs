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
    await foreach (var file in project.Files)
    {
        var fileName = Path.GetFileName(file.Path);
        if (fileName is "package.json" or "yarn.lock" or "package-lock.json" or "lerna.json")
        {
            Console.WriteLine($"\t [ node ] {file.Path}");
        }
        else if (fileName is "Dockerfile" or "docker-compose.yaml" or "docker-compose.yml")
        {
            Console.WriteLine($"\t [docker] {file.Path}");
        }
        else if (fileName is "pom.xml")
        {
            Console.WriteLine($"\t [ java ] {file.Path}");
        }
        else if (fileName is "pyproject.toml")
        {
            Console.WriteLine($"\t [python] {file.Path}");
        }
        else
        {
            //Console.WriteLine($"\t        {file.Path}");
        }
    }
}
