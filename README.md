# Configs
A simple config file API with type serialization, primarily used for games. In the case of use in Unity, the Type and Value fields are serialized and hot-reload friendly.

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

## Loading
```cs
Config audioSettings = Config.Load("audioSettings");
float volume = (float)audioSettings["volume"];
float pan = (float)audioSettings["pan"];
float pitch = (float)audioSettings["pitch"];
```

## Saving
```
Config audioSettings = Config.Load("audioSettings");
audioSettings["volume"] = 0f;
audioSettings.Save();
```

## Format
The format in use follows the `name:type=value` pattern. If a type isnt given, it will assume a string.

## Settings
The settings class provides the ability to customize some of the behaviour of the configs. The `Config` class has a static property that uses the Settings, if none is provided, it will use a default one instead.

**Extension**: The extension to use when saving and loading config files, default is `cfg`.

**ThrowsExceptions**: If this is set to true, it will throw exception on errors.

**Directory**: This is the default directory to use when saving and loading, if set to null, it will use the base directory of the executable.

**TypeDelimeter**: The character used for splitting the name from the type field, default is `:`.

**ValueDelimeter**: The character used for splitting the type from the value, default is `=`.
