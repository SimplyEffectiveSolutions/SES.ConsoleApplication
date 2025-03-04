# Test Cases

Each test case folder contains:

1. `appsettings.json` - The base configuration file
2. `override.json` (optional) - An override configuration file
3. `expected.txt` - The expected output of the command

## Test Cases Description

- **Case1**: Base configuration only (no overrides, no command line args)
  - Command: `echo -m "TestMessage"`

- **Case2**: Using override file
  - Command: `echo -m "TestMessage" --config override.json`

- **Case3**: Command line args overriding base config
  - Command: `echo -m "TestMessage" -k "CommandLineKey"`

- **Case4**: Command line args + override file
  - Command: `echo -m "TestMessage" -k "CommandLineKey" --config override.json`