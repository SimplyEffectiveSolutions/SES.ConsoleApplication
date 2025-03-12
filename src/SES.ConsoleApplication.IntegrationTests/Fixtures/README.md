# Test Fixtures

This directory contains fixture classes used by the integration tests. Fixtures are responsible for setting up and tearing down test environments.

## Overview

A fixture is a class that implements IDisposable and is used with xUnit's ICollectionFixture<T> interface. When a test class is marked with [Collection("Fixture Name")], xUnit will create a single instance of the fixture for all tests in that collection.

## Existing Fixtures

- **BasicFixture**: Sets up a basic test environment for Echo command tests

## Creating a New Fixture

To create a new fixture:

1. Create a new class that implements IDisposable
2. In the constructor, set up the test environment (e.g., copy fixture files, create directories)
3. In the Dispose method, clean up the test environment
4. Create a new collection class for the fixture

Example:

```csharp
// Fixture class
public class MyFixture : IDisposable
{
    public MyFixture()
    {
        // Setup code
    }

    public void Dispose()
    {
        // Cleanup code
    }
}

// Collection class
[CollectionDefinition("My Collection")]
public class MyCollection : ICollectionFixture<MyFixture>
{
    // This class acts as a marker for the collection fixture
}

// Test class using the fixture
[Collection("My Collection")]
public class MyTests
{
    private readonly MyFixture _fixture;

    public MyTests(MyFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public void Test1()
    {
        // Test code
    }
}
```

## Using Fixtures with Test Data

Fixtures should work together with the test data structure. Each fixture should have a corresponding folder in the `TestData/Fixtures` directory that contains the files needed for the fixture.