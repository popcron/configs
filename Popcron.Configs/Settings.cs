namespace Popcron.Configs
{
    public class Settings
    {
        /// <summary>
        /// Extension for files, default is cfg
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Setting for whether obvious errors should throw exceptions
        /// </summary>
        public bool ThrowsExceptions { get; set; }

        /// <summary>
        /// Executing directory to use, default is the base directory of the executable
        /// </summary>
        public string Directory { get; set; }

        public char TypeDelimeter { get; set; }
        public char ValueDelimeter { get; set; }
    }
}