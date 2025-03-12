# Test Data Structure

This directory contains all the test data used by the integration tests. The structure is designed to provide a clear organisation pattern that scales well as more commands, fixtures, and tests are added.

## Migration from Legacy Structure

This new structure replaces the legacy TestCases folder structure. The old structure had:
- A folder for each test case (Case1, Case2, etc.)
- Input and expected output files in each folder

The new structure organizes test data hierarchically by command, fixture, and scenario:
- Commands are now grouped together (Echo, Add, etc.)
- Fixtures provide reusable test environments
- Scenarios represent different test conditions (When...)
- Test names describe the expected behavior (Should...)

## Folder Structure

The test data is organised in a hierarchical structure following the pattern:

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

### Fixtures

The `Fixtures` folder contains shared test environments that can be reused across multiple tests. Each fixture represents a specific test scenario or environment setup.

### Commands

The `Commands` folder contains the input data for each command test, organised by command name, fixture name and test name.

### ExpectedLogs

The `ExpectedLogs` folder contains the expected console output logs for each test, organised by command name, fixture name, and scenario name.

### ExpectedResults

The `ExpectedResults` folder contains the expected file states after a command has been executed, organised by command name, fixture name, scenario name, and test name.

## Test Naming Convention

Test methods in code should follow this naming convention:

```csharp
[Command]_[Fixture]_[ScenarioName]_[TestName]
```

For tests that don't require a specific fixture, use:

```csharp
[Command]_NoFixture_[ScenarioName]_[TestName]
```

## Examples

### Directory Structure Example

NOTE: Fixtures and Commands folders don't have scenario names. (These folders typically keep the input data for a test. The scenario and test name will often modify this data, and that is what we want to check in the ExpectedLogs and ExpectedResults folders.

```
TestData/
├── Fixtures/
│   ├── BasicDiagram/                   # Basic diagram fixture
│   │   ├── DIAG001/
│   │   │   ├── COMP001/
│   │   │   │   └── Description.md
│   │   │   └── COMP002/
│   │   │       └── Description.md
│   │   └── diagram1.dgr
│   └── ComplexDiagram/                 # More complex diagram fixture
│
├── Commands/
│   ├── AddFrontmatter/
│   │   ├── BasicDiagram/
│   │   │   │   └── ShouldAddToEmpty/
│   │   │   │       └── DIAG001/
│   │   │   │           └── COMP003/
│   │   │   │               └── Description.md
│   │   │       └── ShouldSkipExisting/
│   │   └── ComplexDiagram/
│   │           └── ShouldAddToNestedComponents/
│   └── DeleteOrphanedFolders/
│       └── NoFixture/                  # Test without a specific fixture
│               └── ShouldDeleteEmptyFolders/
│                   └── parent/
│                       ├── empty/
│                       └── not_empty/
│                           └── file.txt
│
├── ExpectedLogs/
│   ├── AddFrontmatter/
│   │   ├── BasicDiagram/
│   │   │   ├── WhenWriteEnabled/        # Scenario name
│   │   │   │   └── ShouldAddToEmpty.verified.log
│   │   │   └── WhenReadOnly/            # Scenario name
│   │   │       └── ShouldSkipExisting.verified.log
│   │   └── ComplexDiagram/
│   │       └── WhenWriteEnabled/        # Scenario name
│   │           └── ShouldAddToNestedComponents.verified.log
│   └── DeleteOrphanedFolders/
│       └── NoFixture/
│           └── WhenWriteEnabled/        # Scenario name
│               └── ShouldDeleteEmptyFolders.verified.log
│
└── ExpectedResults/
    ├── AddFrontmatter/
    │   ├── BasicDiagram/
    │   │   ├── WhenWriteEnabled/        # Scenario name
    │   │   │   └── ShouldAddToEmpty/
    │   │   │       └── DIAG001/
    │   │   │           └── COMP003/
    │   │   │               └── Description.verified.md
    │   │   └── WhenReadOnly/            # Scenario name
    │   │       └── ShouldSkipExisting/
    │   │           └── DIAG001/
    │   │               ├── COMP001/
    │   │               │   └── Description.verified.md
    │   │               └── COMP002/
    │   │                   └── Description.verified.md
    │   └── ComplexDiagram/
    │       └── WhenWriteEnabled/        # Scenario name
    │           └── ShouldAddToNestedComponents/
    └── DeleteOrphanedFolders/
        └── NoFixture/
            └── WhenWriteEnabled/        # Scenario name
                └── ShouldDeleteEmptyFolders/
                    └── parent/
                        └── not_empty/
                            └── file.txt
```

### Test Method Name Examples

```csharp
public void AddFrontmatter_BasicDiagram_WhenWriteEnabled_ShouldAddToEmpty()
public void AddFrontmatter_BasicDiagram_WhenReadOnly_ShouldSkipExisting()
public void AddFrontmatter_ComplexDiagram_WhenWriteEnabled_ShouldAddToNestedComponents()
public void DeleteOrphanedFolders_NoFixture_WhenWriteEnabled_ShouldDeleteEmptyFolders()
```

## Working with This Structure

When adding a new test:

1. Create a folder for your test inputs in `Commands/[CommandName]/[FixtureName]/[ScenarioName]/[TestName]/`
2. Add your test method following the naming convention: `[Command]_[Fixture]_[ScenarioName]_[TestName]`
3. The first time your test runs, it will create expected logs and results files
4. Verify the expected output manually to ensure it's correct

For tests without a specific fixture, use `NoFixture` as the fixture name.

Note: The [ScenarioName] should typically start with "When" to describe the condition being tested, such as "WhenWriteEnabled" or "WhenReadOnly".