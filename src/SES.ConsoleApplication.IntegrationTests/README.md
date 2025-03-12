# Integration Tests

This project contains integration tests for the SES.ConsoleApplication.

## Overview

The integration tests are structured in three main components:

1. **Fixtures**: Reusable test environments that serve as starting points for tests
2. **Test Classes**: Classes that inherit from BaseCommandTest to run commands and verify results
3. **Test Data**: Organized folders for fixtures, inputs, and expected outputs

## Key Classes

- **BaseCommandTest**: Base class for all command integration tests that provides helper methods
- **ConfigurationIntegrationTests**: Tests for the Echo command that verify configuration works correctly
- **BasicFixture**: Fixture that sets up a basic test environment for Echo command tests

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

## Test Structure

```
- Commands/                   # Command test classes
  - Base/                     # Base classes for tests
    - BaseCommandTest.cs      # Core test base class
  - ConfigurationIntegrationTests.cs  # Echo command tests
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