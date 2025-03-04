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
- Logging: Use ZLogger for structured logging (ZLog* methods or ConsoleApp.Log)
- Configuration: Follow standard .NET patterns with environment-specific appsettings files
- Nullability: Default is disabled (Nullable=disable)
- Error handling: Use logging for errors, throw exceptions for unrecoverable errors
- Command parameters: Document with XML comments and use parameter validation attributes

## Project Structure
- Commands/ - Application commands (each command = separate class)
- Filters/ - Cross-cutting middleware pipeline components
- Options/ - Configuration option classes