
To see the help message outputed by the command line, run: `dotnet run --project SES.ConsoleApplication\ -- echo -h`

# Settings

- Make sure 'appsettings.json' file is set to "Copy to output directory"
  - It wil end up in the same directory as the exe
- To set the environment value at the command line, use this command:
  - `dotnet run --environment Production` // Other values are Stating, Development

# Notes

## Install

In the directory that contains the .template.config folder, run:

`dotnet new install .` or `dotnet new install . --force` (If the template already exists)

To use the installed template, you can then run:

`dotnet new sesconsole` in a new folder. The name of the folder will dictate the project name. Or use `dotnet new sesconsole -n <project name>` to specify the project name

Alternately, you can also pick the `sesconsole` template from Rider

## Logging

You can use `ConsoleApp.Log()` and `ConsoleApp.LogError()` in static classes and methods.
(The context for ConsoleApp.Log... is "Program", which matters if you want to control the log level from appsettings.json)
Or call the service provider `ConsoleApp.ServiceProvider`:

```c#
public static class StaticExample
{
    public static void Log(string msg)
    {
        if (ConsoleApp.ServiceProvider != null)
        {
            // Using ConsoleApp.Log...
            ConsoleApp.LogError(msg); // Can log anywhere, even in static classes!!
            
            // Using ConsoleApp.ServiceProvider
            var logFactory = ConsoleApp.ServiceProvider.GetService<ILoggerFactory>();
            var logger = logFactory.CreateLogger("StaticLogger");
            logger.ZLogInformation($"Logging an information message from a static class using the service provider.");
        }
    }
}
```

NOTE: All logs are written to the `logs` folder.

## Options

The option classes must end with the word "Option"

## Filters

Filters can be attached globally, to specific classes, or specific methods using `UseFilter<T>` or `[ConsoleAppFilter<T>]`.

You can override the exit code using filters.
You can share information between commands and filters, and between multiple different filters.

See: https://github.com/Cysharp/ConsoleAppFramework?tab=readme-ov-file#filtermiddleware-pipeline--consoleappcontext

## Validation

ConsoleAppFramework supports attribute based parameter validation.

# References

- Really good information on logging here (with configuration): https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line

# TODO

- [ ] Add loglevels to test
- [ ] Remove test project from solution file when building project
- [ ] Write logs into a log folder in the main project folder
- [ ] Create a log file for each run
- [ ] Delete logs that are more than a day old
- [x] Change the folder structure
- [ ] Add:
  - gitignore (Review the content of the file)
  - gitattributes (Review the content of the file)
  - gitconfig
  - .editorconfig
  - license.txt
  - [x] README.md
  - [x] CLAUDE.md