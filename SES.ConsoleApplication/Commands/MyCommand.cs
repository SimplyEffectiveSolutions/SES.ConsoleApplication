using ConsoleAppFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SES.ConsoleApplication.Options;
using ZLogger;

namespace SES.ConsoleApplication.Commands;

// TODO: Rename class, method and parameters
//  Possibly leave the config parameter for all commands
public class MyCommand(IConfiguration configuration, IOptions<PositionOptions> options, ILogger<MyCommand> logger)
{
    /// <summary>Print a message to screen.</summary>
    /// <param name="msg">-m, The message.</param>
    /// <param name="key">-k, Key value.</param>
    /// <param name="config">-c, Config file path. The utility will look for a appsettings.json file in the EXE folder by default. You can however override the default settings with your own.</param>
    public void Echo(string msg, string key = "", string config = "")
    {
        using (logger.BeginScope("Scope1"))
        {
            logger.ZLogTrace($"Logging a trace message");
            logger.ZLogDebug($"Logging a debug message");
            logger.ZLogInformation($"Logging an information message");
            logger.ZLogWarning($"Logging a warning message");
            logger.ZLogError($"Logging an error message");
            logger.ZLogCritical($"Logging a critical message");
        }

        logger.BeginScope("Scope2");
        ConsoleApp.Log($"Message: {msg}. Title: {options.Value.Title}, Name: {options.Value.Name}, Key: {key}");
        ConsoleApp.Log($"Using ConsoleApp.Log() to log this message.");
    }
}