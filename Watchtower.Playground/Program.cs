using Watchtower.Integrations.GitLab;
using Watchtower.Playground;

DotEnv.Load(".env");

var gitLabUrl = Environment.GetEnvironmentVariable("GITLAB_URL") ?? "https://gitlab.com/";
var gitLabToken = Environment.GetEnvironmentVariable("GITLAB_TOKEN") ?? "***";
var dataRoot = Environment.GetEnvironmentVariable("STORAGE_DIR") ?? $"{Directory.GetCurrentDirectory()}/data";

var storage = new Storage(dataRoot);

Console.WriteLine($"String data to: {storage.Root}");

var gitlab = GitLabIntegration.Create(gitLabUrl, gitLabToken);

Console.WriteLine($"Listing GitLab projects: {gitLabUrl}");

await foreach (var project in gitlab.ListProjectsAsync())
{
    Console.WriteLine($"{project.Source} {project.Id}: {project.Namespace}/{project.Name} ({project.DefaultBranch})");
    if (storage.IsProjectStoredAsync(project))
    {
        continue;
    }
    await foreach (var file in project.Files)
    {
        var fileName = file.FileName;
        if (fileName is "package.json" or "yarn.lock" or "package-lock.json" or "lerna.json")
        {
            await storage.PutFileAsync(project, file, gitlab.GetProjectFileAsync(project, file));
            Console.WriteLine($"\t [ node ] {file.Path}");
        }
        else if (fileName is "Dockerfile" or "docker-compose.yaml" or "docker-compose.yml")
        {
            await storage.PutFileAsync(project, file, gitlab.GetProjectFileAsync(project, file));
            Console.WriteLine($"\t [docker] {file.Path}");
        }
        else if (fileName is "pom.xml")
        {
            await storage.PutFileAsync(project, file, gitlab.GetProjectFileAsync(project, file));
            Console.WriteLine($"\t [ java ] {file.Path}");
        }
        else if (fileName is "pyproject.toml")
        {
            await storage.PutFileAsync(project, file, gitlab.GetProjectFileAsync(project, file));
            Console.WriteLine($"\t [python] {file.Path}");
        }
        else
        {
            //Console.WriteLine($"\t        {file.Path}");
        }
    }
}

