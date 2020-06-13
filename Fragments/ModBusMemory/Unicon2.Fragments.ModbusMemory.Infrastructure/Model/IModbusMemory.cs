using System;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.ModbusMemory.Infrastructure.Model
{
    public interface IModbusMemory : IDeviceFragment, IDisposable, IDataProviderContainer
    {
    }

}