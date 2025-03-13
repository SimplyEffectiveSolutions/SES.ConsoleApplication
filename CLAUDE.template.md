# Project Development Guidelines

## Worflow

- Read the "requirements" folder to see if there are any tasks
- Summarize the task
- Explain how you will complete the task and how you will break down into smaller tasks if necessary before writing any code

## Development Guidelines

### Code Style Guidelines
- Commands: Place in Commands/ folder, use constructor injection, must implement command methods
- Options: Place in Options/ folder, must end with "Options" suffix
- Filters: Place in Filters/ folder, implement middleware pattern via UseFilter<T>
- Logging: Use ZLogger for structured logging (ZLog* methods)
- Configuration: Follow standard .NET patterns with environment-specific appsettings files
- Nullability: Default is disabled (Nullable=disable)
- Error handling: Use logging for errors, throw exceptions for unrecoverable errors
- Command parameters: Document with XML comments and use parameter validation attributes
- When checking for NULL use `is Null` rather than `== Null`

## Project Structure
- Commands/ - Application commands (each command = separate class)
- Filters/ - Cross-cutting middleware pipeline components
- Options/ - Configuration option classes

### Code Modifications

- Always make **minimal** changes required to accomplish the task
- When refactoring, focus on specific areas rather than extensive rewrites
- Get explicit approval from user before committing any changes
- Show diffs before staging and committing files
- Never rewrite entire files when targeted edits will suffice
- Preserve existing formatting, whitespace, and comments
- Write an integration or unit test for all new features
- For each new command create a new profile in the Properties/launchSettings.json file

### Git Workflow

- **IMPORTANT:** Never commit changes automatically without explicit approval from the user
- **CRITICAL:** Always run all tests before committing any changes (`dotnet test`) and verify they PASS
- **CRITICAL:** Never EVER commit code that breaks existing tests - this is a blocker that must be fixed first
- **IMPORTANT:** Never commit changes automatically without explicit approval from the user
- **IMPORTANT:** Always explicitly ask whether feature branches should be merged with `--no-ff` (to preserve branch history) or with fast-forward (for linear history)
- Default to `--no-ff` for feature branches to make it clearer what was worked on in each feature

## Code Architecture

- The codebase implements a layered architecture with a clear inheritance hierarchy to maximize code reuse and minimize duplication.
- **IMPORTANT:** Prefer loosely coupled code that is easy to test, read and maintain.

## Instructions

### File Handling

- Always use Path.Combine() for path construction rather than string concatenation with hardcoded separators
- Use relative paths in configuration files whenever possible to improve cross-platform compatibility
- Prefer using System.IO methods over platform-specific file manipulation libraries when working across platforms

### Commands and Options Pattern

- ConsoleAppFramework is used for command-line parsing and execution
- All commands inherit from BaseCommand<TOptions> or specialized base classes
- Options classes use CLSCompliant attributes for command-line binding
- Commands run in read-only mode by default, requiring explicit write flag (-w) to modify files
- Commands follow the template method pattern with standardized execution flow
- Command results provide standardized metrics (processed, updated, error counts)

### Test Structure

#### TestData Folder Organization

The test data is organized in a standard structure to promote consistency and maintainability:

```
TestData/
  ├── Commands/                   # Test input files organized by command
  │   └── CommandName1/           # Each command has its own folder
  │       └── FixtureName/        # Each fixture has its own folder
  │           └── TestName/       # Each test has its own folder with input files
  ├── ExpectedLogs/               # Expected log output files
  │   └── CommandName1/
  │       └── FixtureName/
  │           ├── WhenReadOnly/   # Scenarios for read-only mode
  │           │   └── TestName.verified.log
  │           └── WhenWriteEnabled/ # Scenarios for write mode
  │               └── TestName.verified.log
  ├── ExpectedResults/            # Expected file state after command execution
  │   └── CommandName1/
  │       └── FixtureName/
  │           ├── WhenReadOnly/
  │           │   └── TestName/
  │           │       └── file.verified.ext
  │           └── WhenWriteEnabled/
  │               └── TestName/
  │                   └── file.verified.ext
  └── Fixtures/                   # Shared baseline test environments
      ├── FixtureName1/           # Each fixture is a complete test environment
      └── FixtureName2/
```

This structure allows for:
- Clear separation between input, expected output, and test fixtures
- Consistent naming conventions across all tests
- Easy addition of new tests by following the established pattern
- Automatic verification of both console output and file changes

#### Cross-Platform Testing Considerations

When writing tests that involve file operations, follow these guidelines:


1. **Path Construction**: Use `Path.Combine()` rather than hardcoded separators
   ```csharp
   // Good
   var path = Path.Combine(baseDir, "subfolder", "file.txt");
   
   // Bad
   var path = baseDir + "/subfolder/file.txt";
   var path = baseDir + "\\subfolder\\file.txt";
   ```

2. **Configuration Paths**: Use relative paths in test configuration files

3. **File Operations**: Use platform-agnostic file operations

### Test Naming Convention

Test methods follow this naming pattern:
```
[Command]_[Fixture]_[ScenarioName]_[TestName]
```

For example:
```csharp
public void CommandName_BasicFixture_WhenWriteEnabled_ShouldUpdateFiles()
public void CommandName_ComplexFixture_WhenReadOnly_ShouldDetectIssues()
```

The [ScenarioName] component typically starts with "When" to describe the condition being tested, such as:
- WhenWriteEnabled - Tests the command with the -w flag set to true
- WhenReadOnly - Tests the command without the -w flag (default behavior)