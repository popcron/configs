using System;
using System.IO;
using System.Xml.Serialization;

namespace Popcron.Configs
{
    [Serializable]
    public class Variable
    {
        private string key;

        private string serializedValue;
        private object objectValue;

        private Type type;
        private string typeName;

        public string Key
        {
            get
            {
                return key;
            }
            set
            {
                key = value;
            }
        }

        public object Value
        {
            get
            {
                if (objectValue == null && serializedValue != null)
                {
                    objectValue = Deserialize(serializedValue);
                }

                return objectValue;
            }
            set
            {
                objectValue = value;
                serializedValue = value?.ToString();

                if (value != null)
                {
                    type = value.GetType();
                    typeName = Helper.GetName(type);
                }
                else
                {
                    serializedValue = null;
                }
            }
        }

        public Type Type
        {
            get
            {
                if (type == null) Helper.GetType(typeName);

                return type;
            }
        }

        public Variable(string name, object value)
        {
            this.key = name;

            //set value
            this.objectValue = value;
            this.serializedValue = value?.ToString();

            //set type
            this.type = value.GetType();
            typeName = Helper.GetName(type);
        }

        public override string ToString()
        {
            return Key + " = " + Value.ToString() + "(" + typeName + ")";
        }

        private static Variable Deserialize(string text)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Variable));
            using (StringReader textReader = new StringReader(text))
            {
                return (Variable)xmlSerializer.Deserialize(textReader);
            }
        }

        private static string Serialize(Variable setting)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Variable));
            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, setting);
                return textWriter.ToString();
            }
        }
    }
}
