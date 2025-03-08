using System.Diagnostics;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.VisualBasic.FileIO;
using Xunit;
using SearchOption = System.IO.SearchOption;

namespace SES.ConsoleApplication.IntegrationTests;

public class ConfigurationIntegrationTests : IDisposable
{
    private static readonly string _testCasesDirectory =
        @"E:\Source\SES.ConsoleApplication\src\SES.ConsoleApplication.IntegrationTests\TestCases";

    private static string _publishedExePath;
    
    private static string _applicationProjectDir = @"E:\Source\SES.ConsoleApplication\src\SES.ConsoleApplication";
    
    // Single shared test directory for all tests
    private static string _testDirectory;
    private static string _executablePath;

    static ConfigurationIntegrationTests()
    {
        var testDirectory = Path.Combine(Path.GetTempPath(), "SESConsoleTests");
        
        Directory.Delete(testDirectory, true);
        Directory.CreateDirectory(testDirectory);

        _testDirectory = Path.Combine(testDirectory, $"TestRun_{DateTime.Now:yyyyMMdd_HHmmss}");

        Directory.CreateDirectory(_testDirectory);
        
        // Copy application files once
        _executablePath = CopyApplicationToTestDirectory();
    }

    [Theory]
    [InlineData("Case1", "echo -m \"TestMessage\"")]
    [InlineData("Case2", "echo -m \"TestMessage\" --config override.json")]
    [InlineData("Case3", "echo -m \"TestMessage\" -k \"CommandLineKey\"")]
    [InlineData("Case4", "echo -m \"TestMessage\" -k \"CommandLineKey\" --config override.json")]
    public void RunCommandTest(string testCaseFolder, string commandArgs)
    {
        // Ensure logs directory exists
        var logsDir = Path.Combine(_testDirectory, "logs", testCaseFolder);
        Directory.CreateDirectory(logsDir);
        
        try
        {
            // Arrange
            var testCasePath = Path.Combine(_testCasesDirectory, testCaseFolder);
            var expectedOutputPath = Path.Combine(testCasePath, "expected.txt");
            var expectedOutput = File.ReadAllText(expectedOutputPath).Trim();
            
            // Clean any previous json files from the exe directory
            CleanJsonFiles(_testDirectory);
            
            // Copy JSON files for this test case to the exe directory
            CopyJsonFilesForCase(testCasePath, _testDirectory);
            
            // Act
            var output = RunApplication(_executablePath, commandArgs, _testDirectory).Trim();
            
            // Write actual output to file for debugging if needed
            File.WriteAllText(Path.Combine(logsDir, "actual_output.txt"), output);

            // Assert
            // Remove timestamps
            expectedOutput = Regex.Replace(expectedOutput, @"^[^[].*\[", "[", RegexOptions.Multiline);
            expectedOutput = Regex.Replace(expectedOutput, @"Elapsed:.*$", string.Empty, RegexOptions.Multiline);
            // Remove elapsed time
            output = Regex.Replace(output, @"^[^[].*\[", "[", RegexOptions.Multiline);
            output = Regex.Replace(output, @"Elapsed:.*$", string.Empty, RegexOptions.Multiline);
            
            Assert.Equal(expectedOutput, output);
        }
        catch (Exception ex)
        {
            // Log the error
            File.WriteAllText(Path.Combine(logsDir, "error.log"), ex.ToString());
            throw;
        }
        finally
        {
            // Always clean up JSON files after the test
            CleanJsonFiles(_testDirectory);
        }
    }
    
    private void CleanJsonFiles(string directory)
    {
        // Remove all appsetting JSON files from the directory
        foreach (var file in Directory.GetFiles(directory, ".json"))
        {
            if (file.EndsWith("runtimeconfig.json"))
            {
                // We need this file to run the exe
                continue;
            }
            
            try 
            {
                File.Delete(file);
            }
            catch (IOException) 
            {
                // If file is locked or can't be deleted, log it but continue
                Console.WriteLine($"Warning: Could not delete file {file}");
            }
        }
    }
    
    private void CopyJsonFilesForCase(string sourcePath, string destPath)
    {
        // Copy only the JSON files specific to this test case to the exe directory
        foreach (var file in Directory.GetFiles(sourcePath, "*.json"))
        {
            var destFile = Path.Combine(destPath, Path.GetFileName(file));
            File.Copy(file, destFile, true);
        }
    }
    
    private static string CopyApplicationToTestDirectory()
    {
        // Build the app
        var buildProcess = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "dotnet",
                Arguments = "publish -c Debug",
                WorkingDirectory = _applicationProjectDir,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        buildProcess.Start();
        buildProcess.WaitForExit();
        
        if (buildProcess.ExitCode != 0)
        {
            throw new Exception("Failed to build the application");
        }
        
        // Check if the published executable exists
        var publishDir = Path.Combine(_applicationProjectDir, "bin", "Debug", "net9.0", "publish");
        var publishedExe = Path.Combine(publishDir, "SES.ConsoleApplication.exe");
        
        if (File.Exists(publishedExe))
        {
            _publishedExePath = publishedExe;
        }
        else
        {
            throw new FileNotFoundException("Application executable not found", _publishedExePath);
        }
        
        //--------------------------------
        // Copy all the files recursively
        var publishedExeDir = Path.GetDirectoryName(_publishedExePath) ?? string.Empty;

        FileSystem.CopyDirectory(publishedExeDir, _testDirectory, overwrite: true);
        
        var destinationExePath = Path.Combine(_testDirectory, "SES.ConsoleApplication.exe");
        return destinationExePath;
    }
    
    private string RunApplication(string exePath, string arguments, string workingDirectory)
    {
        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = exePath,
                Arguments = arguments,
                WorkingDirectory = workingDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            }
        };
        
        var output = new System.Text.StringBuilder();
        process.OutputDataReceived += (sender, args) => 
        {
            if (args.Data != null)
                output.AppendLine(args.Data);
        };
        
        process.ErrorDataReceived += (sender, args) => 
        {
            if (args.Data != null)
                output.AppendLine($"ERROR: {args.Data}");
        };
        
        process.Start();
        process.BeginOutputReadLine();
        process.BeginErrorReadLine();
        process.WaitForExit();
        
        return output.ToString();
    }
    
    public void Dispose()
    {
        // For test debugging, we'll keep the test directory
        // To clean up automatically, uncomment:
        // if (Directory.Exists(_testDirectory))
        // {
        //     Directory.Delete(_testDirectory, true);
        // }
    }
}