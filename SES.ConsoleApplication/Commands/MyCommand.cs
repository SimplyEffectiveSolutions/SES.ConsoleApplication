using ConsoleAppFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZLogger;

namespace SES.ConsoleApplication.Commands;

// inject options
//public class MyCommand(IConfiguration configuration, IOptions<PositionOptions> options, ILogger<MyCommand> logger)
public class MyCommand(IConfiguration configuration, ILogger<MyCommand> logger)
{
    /// <summary>Print a message to screen.</summary>
    /// <param name="msg">-m, The message.</param>
    /// <param name="key">-k, Key value.</param>
    /// <param name="config">-c, Config file path. The utility will look for a appsettings.json file in the EXE folder by default</param>
    public void Echo(string msg, string key = "", string config = "")
    {
        // Call service provider by hand
        if (ConsoleApp.ServiceProvider != null)
        {
            var nonInjectedConfiguration = ConsoleApp.ServiceProvider.GetService<IConfiguration>();
        }

        // options.Value.Title = "test";
        
        // Manually overwrite values calling configuration directly
        // configuration["Key"] = "code overriden key";
        // configuration["Position:Title"] = "code overriden title";
        var myKey = configuration["Key"];

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
        //ConsoleApp.Log($"Message: {msg}. Title: {options.Value.Title}, Name: {options.Value.Name}, Key: {myKey}");
        ConsoleApp.Log($"Using ConsoleApp.Log() to log this message.");
    }
}