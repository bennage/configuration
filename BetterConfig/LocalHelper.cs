namespace BetterConfig
{
    using System;
    using System.IO;
    using System.Linq;

    public static class LocalHelper
    {
        const string FolderName = ".bennage";

        public static void Test(string projectId)
        {
            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var path = Path.Combine(home, FolderName);
            //var folder = Directory.CreateDirectory(path);
            var pathToSettings = Path.Combine(path, projectId + ".yml");
            if (File.Exists(pathToSettings))
            {
                File.ReadAllLines(pathToSettings)
                    .Where(line => !string.IsNullOrWhiteSpace(line))
                    .Select(line => line.Split(':'))
                    .Select(parsed => parsed.Length == 1 ? parsed.Concat(new[] { string.Empty }).ToArray() : parsed)
                    .Select(pair => new { key = pair[0], value = pair[1].Trim() })
                    .ToList()
                    .ForEach(x =>
                    {
                        Environment.SetEnvironmentVariable(
                            variable: x.key,
                            value: x.value,
                            target: EnvironmentVariableTarget.Process);
                    });
            }
        }
    }
}