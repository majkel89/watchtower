namespace Watchtower.Core;
using FsPath = Path;

public class ProjectFile
{
    public required string Path { get; init; }

    public string FileName => FsPath.GetFileName(Path);

    public string? DirPath => FsPath.GetDirectoryName(Path);
}
