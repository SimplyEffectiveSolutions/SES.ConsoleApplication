namespace SES.ConsoleApplication.IntegrationTests.Fixtures;

// NOTE: A fixture will copy the data when the test class is instantiated and delete the data folder in the bin once finished
//  It does not copy data between tests!
public class BasicFixture : IDisposable
{
    private readonly TestConfiguration _config;

    public BasicFixture()
    {
        // Load configuration
        _config = TestConfiguration.Load();

        // Setup test environment
        var currentExeDirectory = Directory.GetCurrentDirectory();
        var binDataPath = Path.Combine(currentExeDirectory, _config.FolderNames.BinDataFolderName);

        // Make sure data directory is deleted before copying new data across
        if (Directory.Exists(binDataPath))
        {
            Directory.Delete(binDataPath, true);
        }

        // Copy test files
        var fixturesDir = Path.Combine(_config.Paths.TestDataPath, "Fixtures");
        CopyDirectory(fixturesDir, binDataPath);
    }

    public void Dispose()
    {
        // Cleanup after all tests
        var currentDirectory = Directory.GetCurrentDirectory();
        var binDataDir = Path.Combine(currentDirectory, _config.FolderNames.BinDataFolderName);

        if (Directory.Exists(binDataDir))
        {
            Directory.Delete(binDataDir, true);
        }
    }

    private void CopyDirectory(string sourceDir, string destinationDir)
    {
        // Create destination directory
        Directory.CreateDirectory(destinationDir);

        // First check if the source directory exists
        if (!Directory.Exists(sourceDir))
        {
            throw new DirectoryNotFoundException($"Source directory does not exist: {sourceDir}");
        }

        // Manual copy for cross-platform compatibility instead of VisualBasic.FileSystem
        foreach (var dirPath in Directory.GetDirectories(sourceDir, "*", SearchOption.AllDirectories))
        {
            var newDirPath = dirPath.Replace(sourceDir, destinationDir);
            Directory.CreateDirectory(newDirPath);
        }

        foreach (var filePath in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
        {
            var newFilePath = filePath.Replace(sourceDir, destinationDir);
            File.Copy(filePath, newFilePath, true);
        }
    }
}
