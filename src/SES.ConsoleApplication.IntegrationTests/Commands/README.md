# Command Integration Tests

This directory contains integration tests for all commands in the application. Each command should have its own test class that inherits from `BaseCommandTest`.

## Creating Tests for a New Command

1. Create a new test class in this directory that inherits from `BaseCommandTest`
2. Set the `CommandName` property in the constructor
3. Create test methods following the naming convention:

```csharp
[Command]_[Fixture]_[ScenarioName]_[TestName]
```

For example:
```csharp
public void Echo_Basic_WhenUsingDefaultConfig_ShouldUseBaseConfig()
```

4. Use the helper methods from `BaseCommandTest` to arrange, act, and assert:
   - `CopyFixtureDataDirectoryToTestDirectory()` - Copies fixture data from TestData/Fixtures
   - `CopyCommandDataDirectoryToTestDirectory()` - Copies command-specific data from TestData/Commands
   - `RunCommand()` - Executes the command and returns the output
   - `VerifyActualLogsAgainstExpected()` - Verifies the command output against expected logs

## Test Structure

```
TestData/
├── Fixtures/                      # Shared baseline environments
├── Commands/                      # Command-specific test inputs
│   └── [CommandName]/
│       └── [FixtureName]/
│           └── [TestName]/
├── ExpectedLogs/                  # Expected command outputs (logs)
│   └── [CommandName]/
│       └── [FixtureName]/
│           └── [ScenarioName]/
│               └── [TestName].verified.log
└── ExpectedResults/               # Expected file states after commands
    └── [CommandName]/
        └── [FixtureName]/
            └── [ScenarioName]/
                └── [TestName]/
```

See `TestData/README.md` for more details on the test data structure.