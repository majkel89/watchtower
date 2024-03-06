namespace Watchtower.Playground;

internal static class DotEnv
{
    public static void Load(string filePath)
    {
        var root = Directory.GetCurrentDirectory();
        var dotenv = Path.Combine(root, filePath);

        if (!File.Exists(dotenv))
            throw new ApplicationException($"Dot.env file does not exists: {dotenv}");

        foreach (var line in File.ReadAllLines(dotenv))
        {
            var parts = line.Split(
                '=',
                StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length != 2)
                continue;

            Environment.SetEnvironmentVariable(parts[0], parts[1]);
        }
    }
}
