namespace Watchtower.Core;

public sealed class Project
{
    public required string Source { get; init; }
    public required string Id { get; init; }
    public required string DefaultBranch { get; init; }
    public required string GitUrl { get; init; }
    public required string Name { get; init; }
    public required string Namespace { get; init; }
    public required IAsyncEnumerable<ProjectFile> Files { get; init; }
}
