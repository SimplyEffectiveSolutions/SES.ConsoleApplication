# SES.ConsoleApplication.UnitTests

This project contains unit tests for the SES.ConsoleApplication project. The tests are organized by component type, mirroring the structure of the main application.

## Project Structure

- **Commands/** - Tests for command classes
- **Filters/** - Tests for filter middleware components
- **Options/** - Tests for configuration option classes (when needed)

## Test Guidelines

1. **Test Organization**:
   - Organize tests by component type, mirroring the structure of the main project
   - Name test classes with the suffix "Tests" (e.g., `MyBasicCommandTests`)
   - Group related tests within the same file

2. **Test Naming**:
   - Use descriptive method names in the format: `MethodName_Scenario_ExpectedResult`
   - Example: `Run_WithMessageAndKey_LogsInformation`

3. **Test Structure**:
   - Follow the Arrange-Act-Assert (AAA) pattern
   - Use clear comments to separate test sections
   - Initialize necessary mocks and dependencies in the constructor

4. **Mocking**:
   - Use Moq for mocking dependencies
   - Prefer real objects over mocks when testing is simpler (e.g., for DTO objects)
   - Use Microsoft.Extensions.Options.Options.Create() for creating option objects

5. **Testing Internal Types**:
   - The main project uses `InternalsVisibleTo` to allow testing of internal classes
   - For complex internal dependencies, consider using simplified test stubs

## Running Tests

```bash
# Run all tests
dotnet test

# Run tests with verbosity
dotnet test -v n

# Run a specific test
dotnet test --filter "FullyQualifiedName=SES.ConsoleApplication.UnitTests.Commands.MyBasicCommandTests.Run_WithMessageAndKey_LogsInformation"
```