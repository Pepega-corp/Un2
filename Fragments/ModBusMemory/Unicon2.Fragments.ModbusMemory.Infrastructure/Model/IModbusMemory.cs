using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.Model
{
    public interface IModbusMemory:IDeviceFragment, IDisposable,IDataProviderContaining
    {
        List<IModbusMemoryEntity> CurrentValues { get; set; }
        Action<IDataProvider> DataProviderChanged { get; set; }
        IDataProvider GetDataProvider();
    }

}