
To see the help message outputed by the command line, run: `dotnet run --project SES.ConsoleApplication\ -- echo -h`

# Settings

- Make sure 'appsettings.json' file is set to "Copy to output directory"
  - It will end up in the same directory as the exe
- To set the environment value at the command line, use this command:
  - `dotnet run --environment Production` // Other values are Staging, Development
- TODO: Setup launchSettings.json
	- `dotnet run --launch-pofile <NAME>` or use `-lp`
	- This file located in the project Properties folder

# Notes

## Running command

When running a command with a `bool? param = null` (i.e. a nullable value), only the parameter key is needed. i.e. `command-name --param` or `command-name -p`. (Using `command-name -p true` will cause the command to fail).

## Install

In the directory that contains the .template.config folder, run:

`dotnet new install .` or `dotnet new install . --force` (If the template already exists)

To use the installed template, you can then run:

`dotnet new sesconsole` in a new folder. The name of the folder will dictate the project name. Or use `dotnet new sesconsole -n <project name>` to specify the project name

Alternately, you can also pick the `sesconsole` template from Rider

## Logging

NOTE: (2025-03-09) neuecc recommends not using `ConsoleApp.Log()` and `ConsoleApp.LogError()` in typical applications.

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

## Publishing

Run the `publish.cmd` command.

If you want to change the exe name, update the `publish.cmd` file with: `-p:AssemblyName=YourDesiredExeName` e.g. `dotnet publish -c Release -p:AssemblyName=YourDesiredExeName`

# Utility Scripts

The project includes several utility CMD scripts in the root directory to simplify common tasks:

| Script | Description |
|--------|-------------|
| `setup.cmd` | Initializes a new project after template creation. Creates git repository and necessary directory structure. Automatically executed as a post-action when creating a new project from the template. |
| `publish.cmd` | Publishes the application as a self-contained executable. Use this to create a distributable version of your application. |
| `create_template.cmd` | Utility for template developers to update or regenerate template files. |
| `consolidate_readmes.cmd` | Collects all README.md and README.txt files from the entire project into a single READMES directory. Useful for documentation overview and review. READMES folder is excluded from git |

## Using the Scripts

- **Publishing your application**: Run `publish.cmd` to create a self-contained executable
- **Consolidating documentation**: Run `consolidate_readmes.cmd` to collect all README files into the READMES directory
- **After creating a new project**: `setup.cmd` runs automatically, but can be re-run if needed

# References

- Really good information on logging here (with configuration): https://learn.microsoft.com/en-us/dotnet/core/extensions/logging?tabs=command-line
- .gitignores: https://github.com/github/gitignore
- .editorconfig: https://editorconfig.org/

# TODO

- [ ] Add loglevels to test