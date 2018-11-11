# Configs
A simple config file API with type serialization, primarily used for games.

## Requirements
- .NET Framework 2.0

## Example
```cs
Config config = Config.Load("settings");
string name = config["name"].Value;
DateTime time = DateTime.Now;
if (config.Contains("time"))
{
    time = config["time"].Value;
}
```
