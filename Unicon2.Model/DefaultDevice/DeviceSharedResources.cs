using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Model.DefaultDevice
{
    [DataContract(Name = nameof(DeviceSharedResources),Namespace = "DeviceSharedResourcesNS")]
    public class DeviceSharedResources : Disposable, IDeviceSharedResources
    {
        public DeviceSharedResources()
        {
            this.SharedResources=new List<INameable>();
        }
        [DataMember(Name = nameof(SharedResources))]
        public List<INameable> SharedResources { get; set; }

        public void AddResource(INameable resource)
        {
            this.SharedResources.Add(resource);
        }

        public void DeleteResource(INameable resource)
        {
            this.SharedResources.Remove(resource);
        }

        public bool IsItemReferenced(string name)
        {
            return this.SharedResources.Any(nameableResource => nameableResource.Name == name);
        }
        

        public void SaveInFile(string path, ISerializerService serializerService)
        {
            try
            {
                using (XmlWriter fs = XmlWriter.Create(path, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(DeviceSharedResources), serializerService.GetTypesForSerialiation());

                    ds.WriteObject(fs, this, serializerService.GetNamespacesAttributes());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void LoadFromFile(string path, ISerializerService serializerService)
        {
            try
            {
                using (XmlReader fs = XmlReader.Create(path))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(DeviceSharedResources), serializerService.GetTypesForSerialiation());
                    DeviceSharedResources res = (DeviceSharedResources)ds.ReadObject(fs);
                    this.SharedResources.AddRange(res.SharedResources);
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