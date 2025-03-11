# CLAUDE.md - .NET Console Application Guidelines

## Build & Run Commands
```bash
# Build the project
dotnet build

# Publish the application to a single executable
dotnet publish -c Release -r win-x64 --self-contained

# Run the application
dotnet run --project SES.ConsoleApplication

# Run with specific command
dotnet run --project SES.ConsoleApplication -- echo -m "message" -k "key"

# Run with environment configuration
dotnet run --environment Production

# Run with specific config file
dotnet run --project SES.ConsoleApplication -- echo -m "message" --config path/to/config.json

# Run integration tests
dotnet test
```

## Code Style Guidelines
- Commands: Place in Commands/ folder, use constructor injection, must implement command methods
- Options: Place in Options/ folder, must end with "Options" suffix
- Filters: Place in Filters/ folder, implement middleware pattern via UseFilter<T>
- Logging: Use ZLogger for structured logging (ZLog* methods)
- Configuration: Follow standard .NET patterns with environment-specific appsettings files
- Nullability: Default is disabled (Nullable=disable)
- Error handling: Use logging for errors, throw exceptions for unrecoverable errors
- Command parameters: Document with XML comments and use parameter validation attributes

## Project Structure
- Commands/ - Application commands (each command = separate class)
- Filters/ - Cross-cutting middleware pipeline components
- Options/ - Configuration option classes

## Instructions
- Add an integration test for every new feature

## Conversation Summary

We've created a robust .NET Console Application project template with configuration testing capabilities:

### Project Setup (Completed)
- Created a .NET 9.0 console application using ConsoleAppFramework
- Added a CLAUDE.md file with guidelines for build/run commands and code style
- Made the project usable as a template with a .template.config/template.json file
- Added a UseHosted parameter to conditionally use hosted or non-hosted builder mode

### Integration Testing (Current Focus)
- Created an IntegrationTests project to test configuration precedence
- Set up test cases to verify that configuration values are correctly processed from:
  1. Base settings in appsettings.json
  2. Override files specified with --config
  3. Command-line arguments

- Optimized the test execution to:
  - Build and copy the executable only once per test run
  - Clean and copy relevant JSON files to the exe directory between tests
  - Separate logs by test case for easier debugging

### Files We're Working With
- `/SES.ConsoleApplication/Program.cs` - Main program with conditional template builds
- `/.template.config/template.json` - Template configuration
- `/SES.ConsoleApplication.IntegrationTests/ConfigurationIntegrationTests.cs` - Integration tests
- `/SES.ConsoleApplication.IntegrationTests/TestCases/` - Test data and expected outputs
- `/CLAUDE.md` - Developer guidelines and commands

### Next Steps
Potential next steps could include:
1. Adding more test cases for different configuration scenarios
2. Supporting additional command-line parameters in the template
3. Adding more application commands beyond the Echo example
4. Implementing more filter examples for cross-cutting concerns
5. Enhancing the logging functionality with rotation or additional formats