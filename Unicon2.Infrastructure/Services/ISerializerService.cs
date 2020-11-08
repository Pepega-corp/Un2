namespace Unicon2.Infrastructure.Services
{
    public interface ISerializerService
    {
        void SerializeInFile<T>(T objectToSerialize, string fileName);
        T DeserializeFromFile<T>(string filePath);
        string SerializeInString<T>(T objectToSerialize);
    }
    
}