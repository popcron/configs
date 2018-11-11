using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
using System.Text;
using System.ComponentModel;

namespace Popcron.Configs
{
    [Serializable]
    public class Config
    {
        /// <summary>
        /// Settings for the configs. if no settings preset is used, it will use defaults
        /// </summary>
        public static Settings Settings
        {
            get
            {
                if (settings == null) settings = new Settings();

                return settings;
            }
            set
            {
                settings = value;
            }
        }

        private static Settings settings;

        private List<Variable> values = new List<Variable>();
        private readonly string name;

        public string Name
        {
            get
            {
                return name;
            }
        }

        public Config(string configName)
        {
            name = configName;
        }

        public bool Contains(string key)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Key == key)
                {
                    return true;
                }
            }

            return false;
        }

        public bool Add(string key, object value)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Key == key)
                {
                    if (Settings.throwErrors)
                    {
                        throw new DuplicateNameException("Value with key " + key + " already exists");
                    }

                    return false;
                }
            }

            values.Add(new Variable(key, value));
            return true;
        }

        /// <summary>
        /// Removes a variable with a key name
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i].Key == key)
                {
                    values.RemoveAt(i);
                    return true;
                }
            }

            if (Settings.throwErrors)
            {
                throw new KeyNotFoundException("Value with key " + key + " not found");
            }

            return false;
        }

        public int Count
        {
            get
            {
                return values.Count;
            }
        }

        public Variable this[int index]
        {
            get
            {
                if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
                return values[index];
            }
            set
            {
                if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
                values[index] = value;
            }
        }

        public Variable this[string key]
        {
            get
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].Key == key)
                    {
                        return values[i];
                    }
                }

                if (Settings.throwErrors)
                {
                    throw new KeyNotFoundException("Value with key " + key + " not found");
                }
                else
                {
                    return null;
                }
            }
            set
            {
                for (int i = 0; i < values.Count; i++)
                {
                    if (values[i].Key == key)
                    {
                        values[i] = value;
                        return;
                    }
                }

                if (Settings.throwErrors)
                {
                    throw new KeyNotFoundException("Value with key " + key + " not found");
                }
            }
        }

        /// <summary>
        /// Returns all config files at a directory
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static List<string> GetAllConfigs(string path = null)
        {
            List<string> configs = new List<string>();

            path = path ?? Settings.directory;
            string[] files = Directory.GetFiles(path, "*." + Settings.extension, SearchOption.AllDirectories);
            for (int i = 0; i < files.Length; i++)
            {
                configs.Add(Path.GetFileNameWithoutExtension(files[i]));
            }

            return configs;
        }

        /// <summary>
        /// Loads a config file with a name
        /// </summary>
        /// <param name="configName"></param>
        public static Config Load(string configName, string path = null)
        {
            path = path ?? Settings.directory;
            path += configName + "." + Settings.extension;

            if (File.Exists(path))
            {
                using (StreamReader streamReader = File.OpenText(path))
                {
                    string text = streamReader.ReadToEnd();
                    return Deserialize(text, configName);
                }
            }
            else
            {
                if (Settings.throwErrors)
                {
                    throw new FileNotFoundException("Config by the name of " + configName + " not found at " + path);
                }
            }

            return null;
        }

        /// <summary>
        /// Creates a new config file
        /// </summary>
        /// <param name="configName"></param>
        /// <param name="config"></param>
        /// <returns>The path to the new file</returns>
        public static string Save(Config config, string path = null)
        {
            if (config == null)
            {
                if (Settings.throwErrors)
                {
                    throw new NullReferenceException("Config is null");
                }
                return null;
            }

            //path resolve
            path = path ?? Settings.directory;
            path += config.Name + "." + Settings.extension;

            string text = Serialize(config);
            File.WriteAllText(path, text);
            return path;
        }

        /// <summary>
        /// Save this object to a file
        /// </summary>
        /// <returns>Path to new file</returns>
        public string Save()
        {
            return Save(this);
        }

        /// <summary>
        /// Convert this object to a string
        /// </summary>
        /// <returns></returns>
        public string Serialize()
        {
            return Serialize(this);
        }

        public static string Serialize(Config config)
        {
            string text = "";
            char typeDelim = Settings.typeDelimeter;
            char valueDelim = Settings.valueDelimeter;

            for (int i = 0; i < config.values.Count; i++)
            {
                TypeConverter typeConverter = TypeDescriptor.GetConverter(config.values[i].Type);
                if (typeConverter.CanConvertTo(typeof(string)) && typeConverter.CanConvertFrom(typeof(string)))
                {
                    string value = (string)typeConverter.ConvertTo(config.values[i].Value, typeof(string));
                    string typeName = Helper.GetName(config.values[i].Type);
                    string line = config.values[i].Key + typeDelim + typeName + valueDelim + value;
                    if (i == config.values.Count - 1)
                    {
                        text += line;
                    }
                    else
                    {
                        text += line + "\n";
                    }
                }
                else
                {
                    if (Settings.throwErrors)
                    {
                        throw new Exception("Cant serialize " + config.values[i].Type + " to string");
                    }
                }
            }

            return text;
        }

        public static Config Deserialize(string text, string configName)
        {
            Config config = new Config(configName);
            string[] lines = text.Split('\n');
            char typeDelim = Settings.typeDelimeter;
            char valueDelim = Settings.valueDelimeter;
            config.values = new List<Variable>(lines.Length);

            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i].Contains(valueDelim.ToString()))
                {
                    string name = lines[i].Split(valueDelim)[0];
                    string typeName = "string";
                    string value = lines[i].Split(valueDelim)[1];

                    if (lines[i].Contains(typeDelim.ToString()))
                    {
                        name = lines[i].Split(typeDelim)[0];
                        typeName = lines[i].Split(typeDelim)[1].Split(valueDelim)[0];
                        value = lines[i].Split(valueDelim)[1];
                    }

                    Type type = Helper.GetType(typeName);
                    TypeConverter typeConverter = TypeDescriptor.GetConverter(type);
                    if (typeConverter.CanConvertFrom(typeof(string)))
                    {
                        object propValue = typeConverter.ConvertFrom(value);

                        Variable variable = new Variable(name, propValue);
                        config.values.Add(variable);
                    }
                    else
                    {
                        if (Settings.throwErrors)
                        {
                            throw new Exception("Cant deserialize " + type + " from string");
                        }
                    }
                }
            }
            return config;
        }
    }
}
