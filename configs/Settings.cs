using System;

namespace Popcron.Configs
{
    public class Settings
    {
        public string extension = "cfg";
        public bool throwErrors = true;
        public string directory = AppDomain.CurrentDomain.BaseDirectory;
        public char typeDelimeter = ':';
        public char valueDelimeter = '=';
    }
}