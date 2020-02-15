using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Services.SerializerService
{
    public class SerializerService : ISerializerService
    {
        private List<Type> _allTypes;
        private Dictionary<string, string> _attributesNamespacesDictionary;


        public SerializerService()
        {
            this._allTypes = new List<Type>();
            this._attributesNamespacesDictionary = new Dictionary<string, string>();
        }

        public void AddKnownTypeForSerializationRange(IEnumerable<Type> types)
        {
            foreach (Type type in types)
            {
                this.AddType(type);
            }
        }

        public void AddKnownTypeForSerialization(Type type)
        {
            this.AddType(type);
        }

        public void AddNamespaceAttribute(string attributeName, string namespaceString)
        {
            if (this._attributesNamespacesDictionary.ContainsKey(attributeName))
            {
                if (this._attributesNamespacesDictionary[attributeName] != namespaceString)
                    throw new Exception("Попытка добавления простанства имен xml, отличающегося от уже определенного");
                return;
            }

            this._attributesNamespacesDictionary.Add(attributeName, namespaceString);
        }

        public List<Type> GetTypesForSerialiation()
        {
            return this._allTypes;
        }

        public Dictionary<string, string> GetNamespacesAttributes()
        {
            return this._attributesNamespacesDictionary;
        }

        public void SerializeInFile<T>(T objectToSerialize, string fileName)
        {
            try
            {
                using (XmlWriter fs = XmlWriter.Create(fileName, new XmlWriterSettings {Indent = true}))
                {
                    DataContractSerializer ds = new DataContractSerializer(objectToSerialize.GetType(),
                        StaticContainer.Container.Resolve<ISerializerService>().GetTypesForSerialiation());
                    ds.WriteObject(fs, this);
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
                using (XmlReader fs = XmlReader.Create(path))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(T),
                        StaticContainer.Container.Resolve<ISerializerService>().GetTypesForSerialiation());
                    return (T) ds.ReadObject(fs);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new SerializationException();
            }
        }


        private void AddType(Type type)
        {
            if (this._allTypes.Contains(type)) return;
            this._allTypes.Add(type);
        }
    }
}