
# Claude Instructions

## Preferred libraries to use


## Code Style Guidelines
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