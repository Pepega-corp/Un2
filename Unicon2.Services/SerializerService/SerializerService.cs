using System;
using System.IO;
using Newtonsoft.Json;
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
        public string SerializeInString<T>(T objectToSerialize)
        {
            try
            {
                var res = string.Empty;
                using (StringWriter writer = new StringWriter())
                {
                    var jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        TypeNameHandling = TypeNameHandling.All,
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        Formatting = Formatting.Indented
                    };
                    writer.Write(JsonConvert.SerializeObject(objectToSerialize, jsonSerializerSettings));
                    res = writer.ToString();
                }

                return res;
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public byte[] SerializeInBytes<T>(T objToSerialize)
        {
            try
            {
                using(MemoryStream memoryStream = new MemoryStream())
                {
                    using(StreamWriter writer = new StreamWriter(memoryStream))
                    {
                        var jsonSerializerSettings = new JsonSerializerSettings()
                        {
                            TypeNameHandling = TypeNameHandling.All,
                            PreserveReferencesHandling = PreserveReferencesHandling.All,
                            Formatting = Formatting.Indented
                        };

                        writer.Write(JsonConvert.SerializeObject(objToSerialize, jsonSerializerSettings));
                        writer.Flush();
                        return memoryStream.ToArray();
                    }
                }
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public T DeserializeFromBytes<T>(byte[] values)
        {
            try
            {
                using (MemoryStream memoryStream = new MemoryStream(values))
                {
                    using (StreamReader reader = new StreamReader(memoryStream))
                    {
                        var jsonSerializerSettings = new JsonSerializerSettings()
                        {
                            ObjectCreationHandling = ObjectCreationHandling.Replace,
                            TypeNameHandling = TypeNameHandling.All,
                            PreserveReferencesHandling = PreserveReferencesHandling.All,
                            Formatting = Formatting.Indented
                        };

                        return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), jsonSerializerSettings);
                    }
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
                    var jsonSerializerSettings = new JsonSerializerSettings()
                    {
                        ObjectCreationHandling = ObjectCreationHandling.Replace,
                        TypeNameHandling = TypeNameHandling.All,
                        PreserveReferencesHandling = PreserveReferencesHandling.All,
                        Formatting = Formatting.Indented
                    };
                    return JsonConvert.DeserializeObject<T>(reader.ReadToEnd(), jsonSerializerSettings);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
    }
}