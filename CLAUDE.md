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
dotnet run --project SES.ConsoleApplication -- run -m "message" -k "key"

# Run with environment configuration
dotnet run --environment Production

# Run with specific config file
dotnet run --project SES.ConsoleApplication -- run -m "message" --config path/to/config.json

# Run all tests
dotnet test

# Run specific test project
dotnet test src/SES.ConsoleApplication.UnitTests
dotnet test src/SES.ConsoleApplication.IntegrationTests

# Run tests with filtering
dotnet test --filter "FullyQualifiedName~UnitTests"
dotnet test --filter "Category=Unit"
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

### Test Structure
- SES.ConsoleApplication.UnitTests/ - Unit tests for individual components
- SES.ConsoleApplication.IntegrationTests/ - Integration tests for configuration and end-to-end functionality

## Instructions
- Add an integration test for every new feature

## Conversation Summary

We've created a robust .NET Console Application project template with configuration testing capabilities:

### Project Setup (Completed)
- Created a .NET 9.0 console application using ConsoleAppFramework
- Added a CLAUDE.md file with guidelines for build/run commands and code style
- Made the project usable as a template with a .template.config/template.json file
- Added a UseHosted parameter to conditionally use hosted or non-hosted builder mode
