using ConsoleAppFramework;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SES.ConsoleApplication.Options;
using ZLogger;
using ZLogger.Formatters;

namespace SES.ConsoleApplication;

public static class Startup
{
    // Check to see if the --config parameter has been set
    // Typically used to determine whether to include a JSON configuration provider
    private static string SetConfigPath(string[] strings)
    {
        var configIndex = -1;
        var idx = 0;

        foreach (var arg in strings)
        {
            if (arg.StartsWith("--config"))
            {
                configIndex = idx;
            }

            idx++;
        }

        var s = string.Empty;

        if (idx > -1)
        {
            // Get the value associated with --config key
            s = strings[configIndex + 1];
        }

        return s;
    }

    internal static ConsoleApp.ConsoleAppBuilder CreateHostedConsoleAppBuilder(string[] args)
    {
        // Check to see if the --config parameter has been set
        var configPath = Startup.SetConfigPath(args);
        
        // Mapping for arg keys to JSON configuration property name
        var argsMapping = new Dictionary<string, string>();
        argsMapping.Add("-k", "Key");
        // TODO: For new projects: Add mappings if required (e.g. argsMapping.Add("--key", "json:property:key");)
        
        var builder = Host.CreateApplicationBuilder(args); // Set's the current root path by default
        var env = builder.Environment;

        builder.Configuration.Sources.Clear();
        builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
        builder.Configuration.AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true);
        
        if (!string.IsNullOrEmpty(configPath) && File.Exists(configPath))
        {
            builder.Configuration.AddJsonFile(configPath, optional: false, reloadOnChange: true);
        }
        
        builder.Configuration.AddCommandLine(args, argsMapping); // TODO: Update mappings if required
        
        // NOTE: Shows the configuration overrides. This is completely managed by IHost
        //var configDebugView = builder.Configuration.GetDebugView();

        builder.Logging.ClearProviders()
            .AddZLoggerConsole((options, services) =>
                {
                    var config = services.GetRequiredService<IConfiguration>();
        
                    options.IncludeScopes = config.GetValue<bool>("Logging:ZLoggerConsole:IncludeScopes");
                    options.UsePlainTextFormatter(formatter => ConfigureFormatter(formatter, options.IncludeScopes));
                }
            )
            // TODO: Add a new log for each day and delete old logs
            .AddZLoggerFile("log.txt", (options, services) =>
            {
                var config = services.GetRequiredService<IConfiguration>();
        
                options.IncludeScopes = config.GetValue<bool>("Logging:ZLoggerFile:IncludeScopes");
                options.UsePlainTextFormatter(formatter => ConfigureFormatter(formatter, options.IncludeScopes));
            });

        //-------------------
        // TODO: Add options
        builder.Services.Configure<PositionOptions>(builder.Configuration.GetSection("Position"));
        
        var app = builder.ToConsoleAppBuilder();
        return app;
    }

    internal static ConsoleApp.ConsoleAppBuilder CreateNonHostedConsoleAppBuilder(string[] args)
    {
        // Check to see if the --config parameter has been set
        var configPath = Startup.SetConfigPath(args);
        
        // Mapping for arg keys to JSON configuration property name
        var argsMapping = new Dictionary<string, string>();
        // TODO: add mappings if required (e.g. argsMapping.Add("--key", "json:property:key");)
        
        var app = ConsoleApp.Create()
            .ConfigureDefaultConfiguration(
                builder =>
                {
                    // NOTE: The appsetting.json file in the EXE folder is added by default
                    
                    // NOTE: builder does not have an Environment so we can't add the following JSON file. (Like we did with the Hosted version)
                    //builder.AddJsonFile($"appsettings.{builder.Environment}.json", true, true);
                    
                    if (!string.IsNullOrEmpty(configPath) && File.Exists(configPath))
                    {
                        builder.AddJsonFile(configPath, optional: false, reloadOnChange: true);
                    }
                    
                    // Override config file values with the user supplied command line values
                    builder.AddCommandLine(args, argsMapping); // TODO: Update mappings if required
                }
            )
            .ConfigureServices((configuration, services) =>
            {
                services.Clear();
                
                //-----------------------
                // TODO: Add options here
                services.Configure<PositionOptions>(configuration.GetSection("Position"));
                
                services.AddSingleton<IConfiguration>(configuration);
            })
            .ConfigureLogging((config, builder) =>
            {
                builder.ClearProviders()
                .SetMinimumLevel(LogLevel.Trace)
                .AddZLoggerConsole((options, services) =>
                {
                    options.IncludeScopes = config.GetValue<bool>("Logging:ZLoggerConsole:IncludeScopes");
                    options.UsePlainTextFormatter(formatter => ConfigureFormatter(formatter, options.IncludeScopes));
                })
                // TODO: Add a new log for each day and delete old logs
                .AddZLoggerFile("log.txt", (options, services) =>
                {
                    options.IncludeScopes = config.GetValue<bool>("Logging:ZLoggerFile:IncludeScopes");
                    //options.TimeProvider = TimeProvider.System;
                    options.UsePlainTextFormatter(formatter => ConfigureFormatter(formatter, options.IncludeScopes));
                });
            });

        return app;
    }

    private static void ConfigureFormatter(PlainTextZLoggerFormatter formatter, bool areScopesIncluded)
    {
        //formatter.SetPrefixFormatter($"{0:local-longdate} {1:+##;-##;0} [{2:short}]: ({3}{4}{5}): ", // Include timezone
        formatter.SetPrefixFormatter($"{0:local-longdate} [{1:short}]: ({2}{3}{4}): ",
            (in MessageTemplate template, in LogInfo info) 
                => template.Format(
                    info.Timestamp, 
                    //info.Timestamp.Local.Hour, 
                    info.LogLevel, 
                    info.Category, 
                    (info.ScopeState != null && info.ScopeState.IsEmpty || !areScopesIncluded) ? string.Empty : " => ", 
                    info.ScopeState != null && info.ScopeState.IsEmpty ? string.Empty : info.ScopeState?.Properties[0].Value)
                );
        formatter.SetExceptionFormatter((writer, ex) => Utf8StringInterpolation.Utf8String.Format(writer, $"{ex.Message}"));
    }
}