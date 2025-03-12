using System.Text.Json;

namespace SES.ConsoleApplication.IntegrationTests;

public class TestConfiguration
{
    public PathsConfig Paths { get; set; }
    public FolderNamesConfig FolderNames { get; set; }

    public bool VerifyLogs { get; set; }

    public static TestConfiguration Load()
    {
        var currentDirectory = Directory.GetCurrentDirectory();
        var configPath = Path.Combine(currentDirectory, "TestConfig.json");

        if (!File.Exists(configPath))
        {
            // Fall back to the directory where the assembly is located
            var assemblyPath = typeof(TestConfiguration).Assembly.Location;
            var assemblyDir = Path.GetDirectoryName(assemblyPath);
            configPath = Path.Combine(assemblyDir, "TestConfig.json");

            if (!File.Exists(configPath))
            {
                throw new FileNotFoundException($"TestConfig.json not found in {currentDirectory} or {assemblyDir}");
            }
        }

        var json = File.ReadAllText(configPath);
        var config = JsonSerializer.Deserialize<TestConfiguration>(json);

        // Normalize relative paths based on the current directory
        if (config != null)
        {
            if (!string.IsNullOrEmpty(config.Paths.TestDataPath) && !Path.IsPathRooted(config.Paths.TestDataPath))
            {
                config.Paths.TestDataPath = Path.GetFullPath(Path.Combine(currentDirectory, config.Paths.TestDataPath));
            }

            if (!string.IsNullOrEmpty(config.Paths.ExpectedLogsPath) && !Path.IsPathRooted(config.Paths.ExpectedLogsPath))
            {
                config.Paths.ExpectedLogsPath = Path.GetFullPath(Path.Combine(currentDirectory, config.Paths.ExpectedLogsPath));
            }

            if (!string.IsNullOrEmpty(config.Paths.ExpectedResultsPath) && !Path.IsPathRooted(config.Paths.ExpectedResultsPath))
            {
                config.Paths.ExpectedResultsPath = Path.GetFullPath(Path.Combine(currentDirectory, config.Paths.ExpectedResultsPath));
            }
        }

        return config;
    }

    public class PathsConfig
    {
        public string ExpectedLogsPath { get; set; }
        public string ExpectedResultsPath { get; set; }
        public string TestDataPath { get; set; }
    }

    public class FolderNamesConfig
    {
        public string BinDataFolderName { get; set; }
    }
}
