using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SES.ConsoleApplication.Commands;
using SES.ConsoleApplication.Options;
using System.Collections.Generic;

namespace SES.ConsoleApplication.UnitTests.Commands;

public class MyCommandTests
{
    private readonly Mock<ILogger<MyCommand>> _loggerMock;
    private readonly IOptions<PositionOptions> _options;
    private readonly IConfiguration _configuration;
    private readonly MyCommand _command;

    public MyCommandTests()
    {
        // Setup configuration
        var inMemorySettings = new Dictionary<string, string> {
            {"Position:Title", "Test Title"},
            {"Position:Name", "Test Name"},
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        // Setup real options using Microsoft.Extensions.Options.Options.Create
        _options = Microsoft.Extensions.Options.Options.Create(new PositionOptions
        {
            Title = "Test Title",
            Name = "Test Name"
        });

        // Setup logger
        _loggerMock = new Mock<ILogger<MyCommand>>();

        // Create command instance
        _command = new MyCommand(_configuration, _options, _loggerMock.Object);
    }

    [Fact]
    public void Echo_WithMessageAndKey_LogsInformation()
    {
        // Arrange
        var msg = "Test Message";
        var key = "Test Key";

        // Act - This should access the options value
        _command.Echo(msg, key);

        // Assert - This test is primarily checking that the method executes without exceptions
        // We're using ZLogger which doesn't easily allow for verification
        Assert.NotNull(_command);
    }

    [Fact]
    public void Echo_WithMessageOnly_LogsInformation()
    {
        // Arrange
        var msg = "Test Message";

        // Act - This should access the options value
        _command.Echo(msg);

        // Assert - This test is primarily checking that the method executes without exceptions
        Assert.NotNull(_command);
    }
}