using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Formatting = Newtonsoft.Json.Formatting;

namespace Unicon2.Services.SerializerService
{
    public class SerializerService : ISerializerService
    {
        public void SerializeInFile<T>(T objectToSerialize, string fileName)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(fileName))
                {
                    var jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        Formatting = Formatting.Indented
                    };
                    writer.Write(JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public T DeserializeFromFile<T>(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    return JsonConvert.DeserializeObject<T>(reader.ReadToEnd());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new SerializationException();
            }
        }
    }
}