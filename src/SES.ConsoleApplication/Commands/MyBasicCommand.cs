using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SES.ConsoleApplication.Options;
using ZLogger;

namespace SES.ConsoleApplication.Commands;

// TODO: Rename class, method and parameters
//  Possibly leave the config parameter for all commands
public class MyBasicCommand(IConfiguration configuration, IOptions<BasicOptions> options, ILogger<MyBasicCommand> logger)
{
    /// <summary>Run command. Print a message to screen.</summary>
    /// <param name="msg">-m, The message.</param>
    /// <param name="key">-k, Key value.</param>
    /// <param name="config">
    ///     -c, Config file path. The utility will look for a appsettings.json file in the EXE folder by default. You can however override the default settings with your
    ///     own.
    /// </param>
    public void Run(string msg, string key = "", string config = "")
    {
        using (logger.BeginScope("Logging Levels"))
        {
            logger.ZLogTrace($"Logging a trace message");
            logger.ZLogDebug($"Logging a debug message");
            logger.ZLogInformation($"Logging an information message");
            logger.ZLogWarning($"Logging a warning message");
            logger.ZLogError($"Logging an error message");
            logger.ZLogCritical($"Logging a critical message");
        }

        logger.BeginScope("Args and config values");
        logger.ZLogInformation($"Message: {msg}");
        logger.ZLogInformation($"Title: {options.Value.Title}");
        logger.ZLogInformation($"Name: {options.Value.Name}");
        logger.ZLogInformation($"Key: {key}");
    }
}
