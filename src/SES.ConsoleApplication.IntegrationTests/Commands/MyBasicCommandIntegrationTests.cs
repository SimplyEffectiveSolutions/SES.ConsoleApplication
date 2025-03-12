using SES.ConsoleApplication.IntegrationTests.Commands.Base;
using Xunit;
using Xunit.Abstractions;

namespace SES.ConsoleApplication.IntegrationTests.Commands;

/// <summary>
/// Tests that verify configuration is correctly loaded and precedence rules are followed.
/// These tests ensure that command-line parameters override configuration from files.
/// </summary>
[Collection("Basic Collection")]
public class MyBasicCommandIntegrationTests : BaseCommandTest
{
    public MyBasicCommandIntegrationTests(ITestOutputHelper output) : base(output)
    {
    }

    /// <summary>
    /// Verifies that the base configuration from appsettings.json is used
    /// when no overrides are specified.
    /// </summary>
    [Fact]
    public void Run_Basic_WhenUsingDefaultConfig_ShouldUseBaseConfig()
    {
        // Arrange - Copy the basic fixture data which contains appsettings.json
        CopyFixtureDataDirectoryToTestDirectory();

        // Act - Run the echo command with a simple message
        var output = RunCommand("run", new Dictionary<string, string>
        {
            { "-m", "TestMessage" }
        });

        // Assert - Verify the command output matches the expected log
        VerifyActualLogsAgainstExpected(output);
    }

    /// <summary>
    /// Verifies that configuration from an override file is used when
    /// specified with the --config parameter.
    /// </summary>
    [Fact]
    public void Run_Basic_WhenUsingOverrideFile_ShouldUseOverrideFile()
    {
        // Arrange
        CopyFixtureDataDirectoryToTestDirectory();
        CopyCommandDataDirectoryToTestDirectory();

        // Act - Run echo command with config override
        var output = RunCommand("run", new Dictionary<string, string>
        {
            { "-m", "TestMessage" },
            { "--config", "override.json" }
        });

        // Assert
        VerifyActualLogsAgainstExpected(output);
    }

    /// <summary>
    /// Verifies that command-line parameters override configuration from files.
    /// </summary>
    [Fact]
    public void Run_Basic_WhenUsingCommandLine_ShouldUseCommandLine()
    {
        // Arrange
        CopyFixtureDataDirectoryToTestDirectory();

        // Act - Run echo command with command line key override
        var output = RunCommand("run", new Dictionary<string, string>
        {
            { "-m", "TestMessage" },
            { "-k", "CommandLineKey" }
        });

        // Assert
        VerifyActualLogsAgainstExpected(output);
    }

    /// <summary>
    /// Verifies that command-line parameters take precedence over both
    /// base configuration and override files.
    /// </summary>
    [Fact]
    public void Run_Basic_WhenUsingCommandLineAndOverride_ShouldUseCommandLineAndOverride()
    {
        // Arrange
        CopyFixtureDataDirectoryToTestDirectory();
        CopyCommandDataDirectoryToTestDirectory();

        // Act - Run with both override file and command line parameters
        var output = RunCommand("run", new Dictionary<string, string>
        {
            { "-m", "TestMessage" },
            { "-k", "CommandLineKey" },
            { "--config", "override.json" }
        });

        // Assert
        VerifyActualLogsAgainstExpected(output);
    }
}
