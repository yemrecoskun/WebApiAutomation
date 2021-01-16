using System;
using System.IO;
using System.Reflection;
using System.Xml.Serialization;

namespace WebApiAutomation.Models
{
    public class FileEntitiesBase
    {
        string dataFolder = AppDomain.CurrentDomain.GetData("DataDirectory").ToString();

        public FileEntitiesBase()
        {
            Load();
        }

        private void Load()
        {
            var properties = GetProperties();

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var fileName = GetFileName(property.Name);
                    var filePath = GetFilePath(fileName);

                    CreateDataFile(property, filePath);

                    using (StreamReader reader = new StreamReader(filePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(property.PropertyType);

                        var value = serializer.Deserialize(reader);

                        this.GetType().GetProperty(property.Name).SetValue(this, value);
                    }
                }
            }
        }

        public void SaveChanges()
        {
            var properties = GetProperties();

            if (properties != null)
            {
                foreach (var property in properties)
                {
                    var fileName = GetFileName(property.Name);
                    var filePath = GetFilePath(fileName);

                    CreateDataFile(property,filePath);

                    using (StreamWriter reader = new StreamWriter(filePath))
                    {
                        XmlSerializer serializer = new XmlSerializer(property.GetValue(this).GetType());

                        var value = this.GetType().GetProperty(property.Name).GetValue(this);

                        serializer.Serialize(reader, value);
                    }
                }
            }
        }

        private bool CreateDataFile(PropertyInfo property, string path)
        {
            if (!File.Exists(path))
            {
                using (var file = File.Create(path))
                {
                    var instance = Activator.CreateInstance(property.PropertyType);
                    XmlSerializer serializer = new XmlSerializer(property.PropertyType);
                    serializer.Serialize(file, instance);
                }

                return true;
            }

            return false;
        }

        private string GetFileName(string propertyName)
        {
            return $"{propertyName}.xml";
        }

        private string GetFilePath(string fileName)
        {
            return Path.Combine(dataFolder, fileName);
        }

        private PropertyInfo[] GetProperties()
        {
            return this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }
    }
}