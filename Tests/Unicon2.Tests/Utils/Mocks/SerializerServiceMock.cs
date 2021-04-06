using System.Collections.Generic;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Tests.Utils.Mocks
{
    public class SerializerServiceMock:ISerializerService
    {

        public List<object> SerializedObjects=new List<object>();
        
        public void SerializeInFile<T>(T objectToSerialize, string fileName)
        {
            SerializedObjects.Add(objectToSerialize);
        }

        public T DeserializeFromFile<T>(string filePath)
        {
            throw new System.NotImplementedException();
        }

        public string SerializeInString<T>(T objectToSerialize)
        {
            throw new System.NotImplementedException();
        }

        public byte[] SerializeInBytes<T>(T onjToSerialize)
        {
            throw new System.NotImplementedException();
        }

        public T DeserializeFromBytes<T>(byte[] values)
        {
            throw new System.NotImplementedException();
        }
    }
}