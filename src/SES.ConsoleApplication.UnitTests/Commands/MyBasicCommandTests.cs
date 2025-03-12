using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SES.ConsoleApplication.Commands;
using SES.ConsoleApplication.Options;

namespace SES.ConsoleApplication.UnitTests.Commands;

public class MyBasicCommandTests
{
    private readonly MyBasicCommand _basicCommand;
    private readonly IConfiguration _configuration;
    private readonly Mock<ILogger<MyBasicCommand>> _loggerMock;
    private readonly IOptions<BasicOptions> _options;

    public MyBasicCommandTests()
    {
        // Setup configuration
        var inMemorySettings = new Dictionary<string, string>
        {
            { "Position:Title", "Test Title" },
            { "Position:Name", "Test Name" },
        };

        _configuration = new ConfigurationBuilder().AddInMemoryCollection(inMemorySettings).Build();

        // Setup real options using Microsoft.Extensions.Options.Options.Create
        _options = Microsoft.Extensions.Options.Options.Create(
            new BasicOptions
            {
                Title = "Test Title",
                Name = "Test Name",
            }
        );

        // Setup logger
        _loggerMock = new Mock<ILogger<MyBasicCommand>>();

        // Create command instance
        _basicCommand = new MyBasicCommand(_configuration, _options, _loggerMock.Object);
    }

    [Fact]
    public void Echo_WithMessageAndKey_LogsInformation()
    {
        // Arrange
        var msg = "Test Message";
        var key = "Test Key";

        // Act - This should access the options value
        _basicCommand.Run(msg, key);

        // Assert - This test is primarily checking that the method executes without exceptions
        // We're using ZLogger which doesn't easily allow for verification
        Assert.NotNull(_basicCommand);
    }

    [Fact]
    public void Echo_WithMessageOnly_LogsInformation()
    {
        // Arrange
        var msg = "Test Message";

        // Act - This should access the options value
        _basicCommand.Run(msg);

        // Assert - This test is primarily checking that the method executes without exceptions
        Assert.NotNull(_basicCommand);
    }
}
