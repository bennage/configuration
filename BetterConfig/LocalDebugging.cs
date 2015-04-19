namespace BetterConfig
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    public static class LocalDebugging
    {
        const string FolderName = ".bennage";

        public static KeyValuePair<string, Func<string, string>> Strategy()
        {
            //hack: this is merely a proof of concept
            var projectId = (Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly()).GetName().Name;

            var home = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            var path = Path.Combine(home, FolderName);

            var pathToSettings = Path.Combine(path, projectId + ".yml");

            return File.Exists(pathToSettings)
                ? BuildStrategyForLocalYml(pathToSettings)
                : new KeyValuePair<string, Func<string, string>>(
                    string.Format("no local file: {0}", pathToSettings),
                    key => null);
        }

        private static KeyValuePair<string, Func<string, string>> BuildStrategyForLocalYml(string pathToSettings)
        {
            var lookup = NaivelyParseYml(File.ReadAllLines(pathToSettings));

            return new KeyValuePair<string, Func<string, string>>(
                pathToSettings,
                key => lookup.ContainsKey(key) ? lookup[key] : null);
        }

        public static IDictionary<string, string> NaivelyParseYml(string[] lines)
        {
            return lines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line => line.Split(':'))
                .Select(EnsureKeyAndValue)
                .ToDictionary(x => x[0], x => x[1].Trim());
        }

        private static string[] EnsureKeyAndValue(string[] parsed)
        {
            return parsed.Length == 1
                ? parsed.Concat(new[] { string.Empty }).ToArray()
                : parsed;
        }
    }
}