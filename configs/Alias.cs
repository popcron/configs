using System;

namespace Popcron.Configs
{
    public static class Helper
    {
        /// <summary>
        /// Get type from text
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static Type GetType(string text)
        {
            if (text == "void") return typeof(void);
            else if (text == "bool") return typeof(bool);
            else if (text == "byte") return typeof(byte);
            else if (text == "sbyte") return typeof(sbyte);
            else if (text == "char") return typeof(char);
            else if (text == "decimal") return typeof(decimal);
            else if (text == "double") return typeof(double);
            else if (text == "float") return typeof(float);
            else if (text == "int") return typeof(int);
            else if (text == "uint") return typeof(uint);
            else if (text == "long") return typeof(long);
            else if (text == "ulong") return typeof(ulong);
            else if (text == "object") return typeof(object);
            else if (text == "short") return typeof(short);
            else if (text == "ushort") return typeof(ushort);
            else if (text == "string") return typeof(string);

            return Type.GetType(text);
        }

        /// <summary>
        /// Get alias name from type
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetName(Type type)
        {
            if (type == null) return "null";
            else if (type == typeof(void)) return "void";
            else if (type == typeof(bool)) return "bool";
            else if (type == typeof(byte)) return "byte";
            else if (type == typeof(sbyte)) return "sbyte";
            else if (type == typeof(char)) return "char";
            else if (type == typeof(decimal)) return "decimal";
            else if (type == typeof(double)) return "double";
            else if (type == typeof(float)) return "float";
            else if (type == typeof(int)) return "int";
            else if (type == typeof(uint)) return "uint";
            else if (type == typeof(long)) return "long";
            else if (type == typeof(ulong)) return "ulong";
            else if (type == typeof(object)) return "object";
            else if (type == typeof(short)) return "short";
            else if (type == typeof(ushort)) return "ushort";
            else if (type == typeof(string)) return "string";

            return type.FullName;
        }
    }
}
