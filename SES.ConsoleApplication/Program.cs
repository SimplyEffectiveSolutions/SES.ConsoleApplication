// See https://aka.ms/new-console-template for more information

using ConsoleAppFramework;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

//using Microsoft.Extensions.Options;

//-----------
var switchMappings = new Dictionary<string, string>()
{
    { "-k", "--key" },
    // Add more mappings as needed
};
//-----------

var commandLineArgs = new Dictionary<string, string>();

var configIndex = -1;
var idx = 0;

foreach (var arg in args)
{
    if (arg.StartsWith("--config"))
    {
        configIndex = idx;
    }

    idx++;
}

if (idx > -1)
{
    commandLineArgs["configPath"] = args[configIndex + 1];
}

//----------------

//var argsList = args.ToList();
//args = [];

//-----------------

// Package Import: Microsoft.Extensions.Configuration.Json
var app = ConsoleApp.Create();
//Host.CreateApplicationBuilder()
//        .ToConsoleAppBuilder();

    app.ConfigureDefaultConfiguration(
        builder =>
        {
            if (commandLineArgs.TryGetValue("configPath", out var customConfigPath))
            {
                if (File.Exists(customConfigPath))
                {
                    builder.AddJsonFile(customConfigPath, optional: false, reloadOnChange: true);
                }
            }

            builder.AddCommandLine(args);
        }
    )
    .ConfigureServices((configuration, services) =>
    {
        //Package Import: Microsoft.Extensions.Options.ConfigurationExtensions
        services.Configure<PositionOptions>(configuration.GetSection("Position"));
        services.AddSingleton<IConfiguration>(configuration);
    });

//TODO: If a default config file is found mention that it is being used

//
// // Microsoft.Extensions.DependencyInjection
// var services = new ServiceCollection();
// //services.AddTransient<MyService>();
//
// using var serviceProvider = services.BuildServiceProvider();
//
// // Any DI library can be used as long as it can create an IServiceProvider
// ConsoleApp.ServiceProvider = serviceProvider;

app.Add<MyCommand>();
app.Run(args);

// inject options
public class MyCommand(IConfiguration configuration, IOptions<PositionOptions> options)
{
    public void Echo(string msg, string key ="", string config = "")
    {
        configuration["Key"] = "manual override";
        //configuration["Position:Title"] = "manual override";
        var myKey = configuration["Key"];
        ConsoleApp.Log($"{msg}: {options.Value.Title} - {options.Value.Name} : {myKey}");
        var test = ConsoleApp.ServiceProvider;
        ConsoleApp.Log("hello");
        ConsoleApp.LogError("Error");
    }
}

public class PositionOptions
{
    public string Title { get; set; } = "";
    public string Name { get; set; } = "";
}