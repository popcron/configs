# Configs
A simple config file API with type serialization, primarily used for games.

## Requirements
- .NET Framework 2.0

## Example
```cs
Config config = Config.Load("settings");
string name = config["name"].Value;
DateTime time = DateTime.Now;

//if a time variable exists, then get its value
//otherwise, add the current time as a new variable
if (config.Contains("time"))
{
    time = (DateTime)config["time"].Value;
}
else
{
    config.Add("time", time);
    
    //save as well
    config.Save();
}
```

### Another example
```cs
Config audioSettings = Config.Load("audioSettings");
float volume = (float)audioSettings["volume"];
float pan = (float)audioSettings["pan"];
float pitch = (float)audioSettings["pitch"];
```

## Format
The format in use follows the `name:type=value` pattern. If a type isnt given, it will assume a string.
