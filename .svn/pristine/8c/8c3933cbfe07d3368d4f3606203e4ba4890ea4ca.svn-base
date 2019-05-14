using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Infrastructure.DeviceInterfaces.SharedResources
{
    public interface IDeviceSharedResources:IDisposable
    {
        List<INameable> SharedResources { get; }
        void AddResource(INameable resource);
        void DeleteResource(INameable resource);
        bool IsItemReferenced(INameable nameable);

        void SaveInFile(string path, ISerializerService serializerService);
        void LoadFromFile(string path, ISerializerService serializerService);
    }
}