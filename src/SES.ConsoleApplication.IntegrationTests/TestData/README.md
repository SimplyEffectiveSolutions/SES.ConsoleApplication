# Integration Test Structure

This directory contains the data and expected outputs for all integration tests in the project.

## Overview

The integration tests are structured in three main components:

1. **Fixtures**: Reusable test environments that serve as starting points for tests
2. **Test Classes**: Classes that inherit from BaseCommandTest to run commands and verify results
3. **Test Data**: Organized folders for fixtures, inputs, and expected outputs

## Key Classes

- **BaseCommandTest**: Base class for all command integration tests that provides helper methods
- **BasicFixture**: Fixture that sets up a basic test environment for Echo command tests

## Integration Test Project Structure

```
- Commands/                   # Command test classes
  - Base/                     # Base classes for tests
    - BaseCommandTest.cs      # Core test base class
  - README.md                 # Usage guide
- Fixtures/                   # Fixture classes for test setup
  - BasicFixture.cs           # Basic test fixture
- TestCollections.cs          # xUnit test collections
- TestConfiguration.cs        # Test configuration loader
- TestConfig.json             # Test configuration file
- TestData/                   # Test data folder
  - Commands/                 # Command-specific inputs
  - ExpectedLogs/             # Expected log outputs
  - ExpectedResults/          # Expected file system state after tests
  - Fixtures/                 # Shared test fixtures
  - README.md                 # Detailed data structure guide
```

## TestData Folder Structure

The test data is organised in a hierarchical structure following the pattern:

```
TestData/
├── Fixtures/                      # Shared baseline environments
├── Commands/                      # Command-specific test inputs
│   └── [CommandName]/
│       └── [FixtureName]/
│           └── [TestName]/
│               └── _EXE/          # Special folder - contents copied to executable directory
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

### Fixtures

The `Fixtures` folder contains shared test environments that can be reused across multiple tests. Each fixture represents a specific test scenario or environment setup.

### Commands

The `Commands` folder contains the input data for each command test, organised by command name, fixture name and test name.

### ExpectedLogs

The `ExpectedLogs` folder contains the expected console output logs for each test, organised by command name, fixture name, and scenario name.

### ExpectedResults

The `ExpectedResults` folder contains the expected file states after a command has been executed, organised by command name, fixture name, scenario name, and test name.

### Special _EXE Folder

A special folder named `_EXE` can be included in any test data directory. The contents of this folder will be copied directly to the executable directory (where SES.ConsoleApplication.dll is located), rather than to the destination test directory. This is useful for:

- Configuration files that need to be in the same directory as the executable
- Override files that should be loaded from the executable directory
- Any files that the application expects to find in its own directory

## Working with This Structure

When adding a new test:

1. Create a folder for your test inputs in `Commands/[CommandName]/[FixtureName]/[ScenarioName]/[TestName]/`
2. Add your test method following the naming convention: `[Command]_[Fixture]_[ScenarioName]_[TestName]`
3. The first time your test runs, it will create expected logs and results files
4. Verify the expected output manually to ensure it's correct

For tests without a specific fixture, use `NoFixture` as the fixture name.

Note: The [ScenarioName] should typically start with "When" to describe the condition being tested, such as "WhenWriteEnabled" or "WhenReadOnly".

## Test Naming Convention

Test methods in code should follow this naming convention:

```csharp
[Command]_[Fixture]_[ScenarioName]_[TestName]
```

For tests that don't require a specific fixture, use:

```csharp
[Command]_NoFixture_[ScenarioName]_[TestName]
```

## Running Tests

Tests can be run with:
```bash
dotnet test src/SES.ConsoleApplication.IntegrationTests
```

## Adding New Tests

When adding a new test:

1. Create input data in `TestData/Commands/[CommandName]/[FixtureName]/[TestName]/`
2. Create a test method in an appropriate test class (or create a new class)
3. Run the test to generate verification files
4. Manually verify the output is correct

See the README.md in each folder for more detailed instructions.

### Test Method Name Examples

```csharp
public void AddFrontmatter_BasicDiagram_WhenWriteEnabled_ShouldAddToEmpty()
public void AddFrontmatter_BasicDiagram_WhenReadOnly_ShouldSkipExisting()
public void AddFrontmatter_ComplexDiagram_WhenWriteEnabled_ShouldAddToNestedComponents()
public void DeleteOrphanedFolders_NoFixture_WhenWriteEnabled_ShouldDeleteEmptyFolders()
```
