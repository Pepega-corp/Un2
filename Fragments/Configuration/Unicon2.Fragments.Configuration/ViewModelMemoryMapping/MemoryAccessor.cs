using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public static class MemoryAccessor
    {
        public static ushort[] GetUshortsFromMemory(IDeviceMemory deviceMemory, ushort address,
            ushort numberOfPoints, bool isLocal)
        {
            if (isLocal)
            {
                return GetUshortsFromMemoryDictionary(deviceMemory.LocalMemoryValues, address, numberOfPoints);
            }
            else
            {
                return GetUshortsFromMemoryDictionary(deviceMemory.DeviceMemoryValues, address, numberOfPoints);
            }
        }

        public static void GetUshortsInMemory(IDeviceMemory deviceMemory, ushort address,
            ushort[] values, bool isLocal)
        {
            if (isLocal)
            {
                 SetUshortsInMemoryDictionary(deviceMemory.LocalMemoryValues, address, values);
            }
            else
            {
                SetUshortsInMemoryDictionary(deviceMemory.DeviceMemoryValues, address, values);
            }
        }

        private static void SetUshortsInMemoryDictionary(Dictionary<ushort, ushort> configurationMemoryLocalMemoryValues, ushort address, ushort[] values)
        {
            ushort addressCurrent = address;
            foreach (var value in values)
            {
                if(configurationMemoryLocalMemoryValues.ContainsKey(addressCurrent))
                {
                    configurationMemoryLocalMemoryValues[addressCurrent] = value;
                }
                else
                {
                    configurationMemoryLocalMemoryValues.Add(addressCurrent,value);
                }
                addressCurrent++;
            }
        }


        private static ushort[] GetUshortsFromMemoryDictionary(Dictionary<ushort, ushort> configurationMemoryValues, ushort address, ushort numberOfPoints)
        {
            var result = new ushort[numberOfPoints];
            var counter = 0;
            for (var i = address; i < address + numberOfPoints; i++)
            {
                result[counter++] = configurationMemoryValues[i];
            }
            return result;
        }

    }
}