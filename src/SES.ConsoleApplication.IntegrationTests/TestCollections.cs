using SES.ConsoleApplication.IntegrationTests.Fixtures;
using Xunit;

namespace SES.ConsoleApplication.IntegrationTests;

[CollectionDefinition("Basic Collection")]
public class BasicCollection : ICollectionFixture<BasicFixture>
{
    // This class acts as a marker for the collection fixture.
    // It doesn't need to contain any code.
}
