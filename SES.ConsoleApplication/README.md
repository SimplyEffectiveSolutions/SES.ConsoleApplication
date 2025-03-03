
To see the help message outputed by the command line, run: `dotnet run --project SES.ConsoleApplication\ -- echo -h`

# Settings

- Make sure 'appsettings.json' file is set to "Copy to output directory"
  - It wil end up in the same directory as the exe
- To set the environment value at the command line, use this command:
  - `dotnet run --environment Production` // Other values are Stating, Development

# Notes

## Logging

You can use `ConsoleApp.Log()` and `ConsoleApp.LogError()` in static classes and methods.
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