using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;


namespace BodeAbp.Scaffolding.Utils
{
    internal class StorageMan<T> where T : new()
    {
        public StorageMan(string modelTypeName, string savefolderPath)
        {
            this.ModelName = modelTypeName;
            this.storageBaseDirectory = savefolderPath;
        }

        public string ModelName { get; set; }

        private string m_storageBaseDirectory;
        private string storageBaseDirectory
        {
            get
            {
                //string dir = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data");
                //if (!Directory.Exists(dir))
                //    Directory.CreateDirectory(dir);
                //return dir;
                return m_storageBaseDirectory;
            }
            set
            {
                if (!Directory.Exists(value))
                    Directory.CreateDirectory(value);

                m_storageBaseDirectory = value;
            }
        }

        private string storageFilePath
        {
            get
            {
                return System.IO.Path.Combine(storageBaseDirectory
                    , string.Format("{0}.{1}", ModelName, "json"));
            }
        }

        public void Save(T data)
        {
            using (StreamWriter sw = new StreamWriter(storageFilePath))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(writer, data);
                }
            }
        }

        public T Read()
        {
            if (!System.IO.File.Exists(storageFilePath)) return new T();

            using (StreamReader sr = new StreamReader(storageFilePath))
            {
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    object obj = serializer.Deserialize<T>(reader);

                    return (obj != null ? (T)obj : new T());
                }
            }
        }


        public T StringToObject(string json)
        {
            using (StringReader sr = new StringReader(json))
            {
                using (JsonTextReader reader = new JsonTextReader(sr))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    object obj = serializer.Deserialize<T>(reader);

                    return (obj != null ? (T)obj : new T());
                }
            }
        }
    }
}
