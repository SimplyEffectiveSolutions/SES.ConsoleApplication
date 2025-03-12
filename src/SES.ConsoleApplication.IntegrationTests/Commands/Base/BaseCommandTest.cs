using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using Xunit;
using Xunit.Abstractions;

namespace SES.ConsoleApplication.IntegrationTests.Commands.Base;

public abstract class BaseCommandTest
{
    protected readonly string _binPath;
    protected readonly string _binDataPath;
    protected readonly string _binExecutablePath;
    protected readonly string _expectedLogsPath;
    protected readonly string _expectedResultsPath;
    protected readonly ITestOutputHelper _output;
    protected readonly string _testDataPath;
    protected readonly bool _verifyLogs;

    protected BaseCommandTest(ITestOutputHelper output)
    {
        _output = output;

        // Load configuration
        var config = TestConfiguration.Load();
        _expectedLogsPath = config.Paths.ExpectedLogsPath;
        _expectedResultsPath = config.Paths.ExpectedResultsPath;
        _testDataPath = config.Paths.TestDataPath;
        _verifyLogs = config.VerifyLogs;

        // Get the current directory and navigate to the test bin data
        var currentDirectory = Directory.GetCurrentDirectory();
        _binPath = currentDirectory;
        _binDataPath = Path.Combine(currentDirectory, config.FolderNames.BinDataFolderName);

        // Navigate to the executable path
        _binExecutablePath = Path.Combine(currentDirectory, "SES.ConsoleApplication.dll");

        // Ensure the executable exists
        if (!File.Exists(_binExecutablePath))
        {
            throw new FileNotFoundException($"Executable not found at path: {_binExecutablePath}. Please build the main project first.");
        }
    }

    /// <summary>
    ///     Runs a command and returns the output
    /// </summary>
    protected string RunCommand(string commandName, Dictionary<string, string> parameters)
    {
        // Build the arguments string
        var args = $"{commandName}";
        foreach (var param in parameters)
        {
            if (string.IsNullOrEmpty(param.Value))
            {
                args += $" {param.Key}";
            }
            else
            {
                args += $" {param.Key} {param.Value}";
            }
        }

        _output.WriteLine($"Running command: dotnet {_binExecutablePath} {args}");

        // Run the command
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = $"{_binExecutablePath} {args}",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            },
        };

        var outputBuilder = new StringBuilder();
        var errorBuilder = new StringBuilder();

        process.OutputDataReceived += (sender, data) =>
        {
            if (data.Data != null)
            {
                outputBuilder.AppendLine(data.Data);
                _output.WriteLine(data.Data);
            }
        };

        process.ErrorDataReceived += (sender, data) =>
        {
            if (data.Data != null)
            {
                errorBuilder.AppendLine(data.Data);
                _output.WriteLine($"ERROR: {data.Data}");
            }
        };

        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();

        // Combine output and error
        var output = outputBuilder.ToString();
        var error = errorBuilder.ToString();

        if (!string.IsNullOrEmpty(error))
        {
            output += Environment.NewLine + "ERRORS:" + Environment.NewLine + error;
        }

        return output;
    }

    /// <summary>
    ///     Helper method to copy an entire directory structure from fixture folder to test directory
    ///     Uses the caller method name to determine which test data to copy
    /// </summary>
    protected void CopyFixtureDataDirectoryToTestDirectory([CallerMemberName] string callerMethodName = null)
    {
        // Parse all parts from the method name
        var (_, fixtureName, scenarioName, testName) = ParseTestMethodName(callerMethodName);

        var sourceDirectory = Path.Combine(_testDataPath, "Fixtures", fixtureName);
        var targetDirectory = Path.Combine(_binDataPath, fixtureName);

        // Ensure the source directory exists
        if (!Directory.Exists(sourceDirectory))
        {
            throw new DirectoryNotFoundException($"Source data directory not found: {sourceDirectory}");
        }

        // Delete the target directory to start with a clean slate
        Directory.Delete(targetDirectory, true);

        // Create target directory if it doesn't exist
        Directory.CreateDirectory(targetDirectory);

        // Use our custom copy method that handles _EXE folders
        CopyDirectoryWithSpecialFolders(sourceDirectory, targetDirectory);

        _output.WriteLine($"Copied fixture directory from {sourceDirectory} to {targetDirectory} for test: {testName}");
    }

    /// <summary>
    ///     Helper method to copy an entire directory structure from command folder to test directory
    ///     Uses the caller method name to determine which test data to copy
    /// </summary>
    protected void CopyCommandDataDirectoryToTestDirectory([CallerMemberName] string callerMethodName = null)
    {
        // Parse all parts from the method name
        var (commandName, fixtureName, scenarioName, testName) = ParseTestMethodName(callerMethodName);

        // Source directory path in commands folder
        // Don't use scenario name for now
        var sourceDirectory = Path.Combine(_testDataPath, "Commands", commandName, fixtureName, testName);

        // Get the target directory
        var targetDirectory = Path.Combine(_binDataPath, fixtureName);

        // Ensure the source directory exists
        if (!Directory.Exists(sourceDirectory))
        {
            throw new DirectoryNotFoundException($"Source data directory not found: {sourceDirectory}");
        }

        // Create target directory if it doesn't exist
        Directory.CreateDirectory(targetDirectory);

        // Use our custom copy method that handles _EXE folders
        CopyDirectoryWithSpecialFolders(sourceDirectory, targetDirectory);

        _output.WriteLine($"Copied command directory from {sourceDirectory} to {targetDirectory} for test: {testName}");
    }

    /// <summary>
    /// Copies a directory and its contents to a destination directory.
    /// Special handling for directories named "_EXE" - contents will be copied to the executable directory.
    /// </summary>
    /// <param name="sourceDir">Source directory path</param>
    /// <param name="destDir">Destination directory path</param>
    protected void CopyDirectoryWithSpecialFolders(string sourceDir, string destDir)
    {
        // Create the destination directory if it doesn't exist
        if (!Directory.Exists(destDir))
        {
            Directory.CreateDirectory(destDir);
        }

        // Copy all files from source to destination
        foreach (var file in Directory.GetFiles(sourceDir))
        {
            var fileName = Path.GetFileName(file);
            var destFilePath = Path.Combine(destDir, fileName);
            File.Copy(file, destFilePath, true);
        }

        // Process subdirectories
        foreach (var subDir in Directory.GetDirectories(sourceDir))
        {
            var dirName = Path.GetFileName(subDir);

            // Check if this is a special _EXE folder
            if (dirName.Equals("_EXE", StringComparison.OrdinalIgnoreCase))
            {
                // Copy contents to the exe directory instead of the normal destination
                _output.WriteLine($"Copying _EXE folder contents from {subDir} to executable directory: {_binPath}");

                foreach (var file in Directory.GetFiles(subDir))
                {
                    var fileName = Path.GetFileName(file);
                    var destFilePath = Path.Combine(_binPath, fileName);
                    File.Copy(file, destFilePath, true);
                }
            }
            else
            {
                // Normal directory - process recursively
                var destSubDir = Path.Combine(destDir, dirName);
                CopyDirectoryWithSpecialFolders(subDir, destSubDir);
            }
        }
    }

    /// <summary>
    ///     Parses test method name components from the caller method name.
    ///     Handles both new format (Command_Fixture_Scenario_Test where Scenario starts with "When")
    ///     and legacy format (Command_Fixture_Test without scenario).
    /// </summary>
    protected (string commandName, string fixtureName, string scenarioName, string testName) ParseTestMethodName([CallerMemberName] string methodName = null)
    {
        var parts = methodName.Split('_');

        // We expect the format Command_Fixture_Scenario_Test
        // where Scenario always starts with "When"
        if (parts.Length >= 3)
        {
            var commandName = parts[0];
            var fixtureName = parts[1];

            // Extract the test name
            if (parts.Length >= 4 && parts[2].StartsWith("When"))
            {
                // Command_Fixture_Scenario_Test format
                var scenarioName = parts[2];

                // Get just the test name (last part)
                var lastUnderscorePos = methodName.LastIndexOf('_');
                var testName = methodName.Substring(lastUnderscorePos + 1);

                return (commandName, fixtureName, scenarioName, testName);
            }
            else
            {
                // Legacy Command_Fixture_Test format (no scenario)
                var scenarioName = string.Empty;

                // Extract the test name (everything after the second underscore)
                var secondUnderscorePos = methodName.IndexOf('_', methodName.IndexOf('_') + 1);
                var testName = methodName.Substring(secondUnderscorePos + 1);

                return (commandName, fixtureName, scenarioName, testName);
            }
        }

        throw new Exception($"Method name {methodName} not formatted correctly.");
    }

    /// <summary>
    ///     Verifies that a file matches expected content
    /// </summary>
    /// <param name="filePath">The path to the file to verify</param>
    /// <param name="patternToReplace">Optional regex pattern to replace with empty string before comparison</param>
    /// <param name="callerMethodName">The calling method name (automatically provided)</param>
    protected void VerifyActualFileAgainstExpected(string filePath, string patternToReplace = null, [CallerMemberName] string callerMethodName = null)
    {
        // Extract all components from the method name
        var (commandName, fixtureName, scenarioName, testName) = ParseTestMethodName(callerMethodName);

        // Make sure the file exists
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found at: {filePath}");
        }

        // Read the actual content
        var actualContent = File.ReadAllText(filePath);

        // Apply content replacements if provided
        if (!string.IsNullOrEmpty(patternToReplace))
        {
            actualContent = Regex.Replace(actualContent, patternToReplace, "<REPLACE>");
        }

        // Extract relative folder structure from the file path
        // First determine which part of the path is based on _binDataPath
        var relativePath = filePath;
        if (filePath.StartsWith(_binDataPath))
        {
            // Get the part of the path after _binDataPath
            relativePath = filePath.Substring(_binDataPath.Length).TrimStart(Path.DirectorySeparatorChar);
        }

        // If the path contains the fixture name, extract the part after it
        if (relativePath.StartsWith(fixtureName))
        {
            relativePath = relativePath.Substring(fixtureName.Length).TrimStart(Path.DirectorySeparatorChar);
        }

        var fileName = Path.GetFileNameWithoutExtension(relativePath); //.Replace(".verified", string.Empty);
        var fileExtension = Path.GetExtension(relativePath);
        var expectedPartialPath = relativePath.Replace($"{fileName}{fileExtension}", $"{fileName}.verified{fileExtension}");

        // Construct path to expected file - maintain the folder structure
        var expectedBasePath = Path.Combine(_expectedResultsPath, commandName, fixtureName, scenarioName, testName);
        var expectedFullPath = Path.Combine(expectedBasePath, expectedPartialPath);

        _output.WriteLine($"Checking file against expected content: {expectedFullPath}");

        // Ensure the expected file exists
        if (!File.Exists(expectedFullPath))
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(expectedFullPath));

            var unverifiedPartialLogPath = relativePath.Replace($"{fileName}{fileExtension}", $"{fileName}.unverified{fileExtension}");
            var unverifiedFullLogPath = Path.Combine(expectedBasePath, unverifiedPartialLogPath);

            // Write the current content as the expected output for future runs
            _output.WriteLine($"Expected file not found. Creating an 'unverified' version with the current output: {unverifiedFullLogPath}");

            // NOTE: The user MUST verify that the log is correct, and rename the file to 'verified'
            File.WriteAllText(unverifiedFullLogPath, actualContent);
            return;
        }

        // Read the expected content
        var expectedContent = File.ReadAllText(expectedFullPath);

        // Normalize line endings for cross-platform compatibility before comparison
        var normalizedExpected = expectedContent.Replace("\r\n", "\n");
        var normalizedActual = actualContent.Replace("\r\n", "\n");

        // Check for mismatches
        if (!normalizedExpected.Equals(normalizedActual))
        {
            // For mismatch, create an actual file next to the expected one
            var expectedDirectory = Path.GetDirectoryName(expectedFullPath);
            var actualContentPath = Path.Combine(expectedDirectory, $"{fileName}.mismatch{fileExtension}");

            File.WriteAllText(actualContentPath, actualContent);

            _output.WriteLine($"File content mismatch! Actual content saved to: {actualContentPath}");

            // Launch kdiff3 to compare the files
            _output.WriteLine("Launching kdiff3 to compare expected and actual content...");

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "kdiff3",
                        Arguments = $"\"{expectedFullPath}\" \"{actualContentPath}\"",
                        UseShellExecute = true,
                    },
                };
                process.Start();
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed to launch kdiff3: {ex.Message}");
            }
        }

        Assert.Equal(normalizedExpected, normalizedActual);
    }

    /// <summary>
    ///     Verifies that the command output matches expected output from a log file
    /// </summary>
    protected void VerifyActualLogsAgainstExpected(string actualOutput, string patternToReplace = null, [CallerMemberName] string callerMethodName = null)
    {
        // Users can disable checking logs from the TestConfig.json file
        if (!_verifyLogs)
        {
            return;
        }

        // Parse all parts from the method name
        var (commandName, fixtureName, scenarioName, testName) = ParseTestMethodName(callerMethodName);

        // Apply content replacements if provided
        if (!string.IsNullOrEmpty(patternToReplace))
        {
            actualOutput = Regex.Replace(actualOutput, patternToReplace, "<REPLACE>");
        }

        // Check there is no unverified.log file in the directory first. Otherwise return false immediately.
        // NOTE: unverified.logs MUST be manually verified and renamed first!
        var unverifiedPartialLogPath = Path.Combine(commandName, fixtureName, scenarioName, $"{testName}.unverified.log");
        var unverifiedFullLogPath = Path.Combine(_expectedLogsPath, unverifiedPartialLogPath);

        if (File.Exists(unverifiedFullLogPath))
        {
            throw new Exception($"File {unverifiedFullLogPath} exists. It must be manually checked and renamed to 'verified' first!");
        }

        // Construct path using CommandName, fixtureName, scenarioName, and testName
        var expectedPartialLogPath = Path.Combine(commandName, fixtureName, scenarioName, $"{testName}.verified.log");
        var expectedFullLogPath = Path.Combine(_expectedLogsPath, expectedPartialLogPath);

        _output.WriteLine($"Checking actual output against expected log: {expectedFullLogPath}");

        // Ensure the expected log file exists
        if (!File.Exists(expectedFullLogPath))
        {
            // Create the directory if it doesn't exist
            Directory.CreateDirectory(Path.GetDirectoryName(expectedFullLogPath));

            // Write the current output as the expected output for future runs
            _output.WriteLine($"Expected log file not found. Creating an 'unverified' version with the current output: {unverifiedFullLogPath}");

            // NOTE: The user MUST verify that the log is correct, and rename the file to 'verified'
            File.WriteAllText(unverifiedFullLogPath, actualOutput);
            return;
        }

        // Read the expected output from the file
        var expectedOutput = File.ReadAllText(expectedFullLogPath);

        // Remove timestamps
        // Remove elapsed time
        var originalActualOutput = actualOutput;
        actualOutput = Regex.Replace(actualOutput, @"^[^[].*\[", "[", RegexOptions.Multiline);
        actualOutput = Regex.Replace(actualOutput, @"Elapsed:.*$", string.Empty, RegexOptions.Multiline);
#if LINUX
        actualOutput = NormalizeLogForComparison(actualOutput);
#endif
        actualOutput = actualOutput.Trim();

        expectedOutput = Regex.Replace(expectedOutput, @"^[^[].*\[", "[", RegexOptions.Multiline);
        expectedOutput = Regex.Replace(expectedOutput, @"Elapsed:.*$", string.Empty, RegexOptions.Multiline);
#if LINUX
        expectedOutput = NormalizeLogForComparison(expectedOutput);
#endif
        expectedOutput = expectedOutput.Trim();

        // Normalize line endings for cross-platform compatibility before comparison
        var normalizedExpected = expectedOutput.Replace("\r\n", "\n");
        var normalizedActual = actualOutput.Replace("\r\n", "\n");

        if (!normalizedExpected.Equals(normalizedActual))
        {
            var expectedFileDirectory = Path.GetDirectoryName(expectedFullLogPath);
            var fileName = Path.GetFileNameWithoutExtension(expectedFullLogPath).Replace(".verified", string.Empty);
            var actualLogPath = Path.Combine(expectedFileDirectory, $"{fileName}.mismatch.log");
            File.WriteAllText(actualLogPath, originalActualOutput);

            _output.WriteLine($"Output mismatch! Actual output saved to: {actualLogPath}");

            // Launch kdiff3 to compare the files
            _output.WriteLine("Launching kdiff3 to compare expected and actual output...");

            try
            {
                var process = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "kdiff3",
                        Arguments = $"--cs \"IgnoreNumbers=1\" \"{expectedFullLogPath}\" \"{actualLogPath}\"",
                        UseShellExecute = true,
                    },
                };
                process.Start();
            }
            catch (Exception ex)
            {
                _output.WriteLine($"Failed to launch kdiff3: {ex.Message}");
            }
        }

        Assert.Equal(normalizedExpected, normalizedActual);
    }

    /// <summary>
    /// Normalizes log output for cross-platform comparison by removing platform-specific elements
    /// </summary>
    private string NormalizeLogForComparison(string log)
    {
        if (string.IsNullOrEmpty(log))
            return string.Empty;

        // Remove timestamps and standardize log format
        log = Regex.Replace(log, @"^\d{4}-\d{2}-\d{2} \d{2}:\d{2}:\d{2}\.\d+ ", "", RegexOptions.Multiline);

        // Remove elapsed time information
        log = Regex.Replace(log, @"Elapsed:.*$", string.Empty, RegexOptions.Multiline);

        // Normalize file paths
        // Convert Windows paths to a standard format
        log = Regex.Replace(log, @"[A-Z]:\\Source\\", "/source/", RegexOptions.IgnoreCase);
        log = Regex.Replace(log, @"[A-Z]:\\", "/", RegexOptions.IgnoreCase);
        log = Regex.Replace(log, @"\\", "/");

        // Handle Linux paths (simplify /mnt/drive paths)
        log = Regex.Replace(log, @"/mnt/[a-z]/source/", "/source/", RegexOptions.IgnoreCase);

        // Make all paths lowercase for consistent comparisons
        log = Regex.Replace(log, @"(/[^:\s]+)", match => match.Groups[1].Value.ToLowerInvariant());

        return log.Trim();
    }
}
