using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public class MemoryAccessor
    {
        public static ushort[] GetUshortsFromMemory(IConfigurationMemory configurationMemory, ushort address,
            ushort numberOfPoints, bool isLocal)
        {
            if (isLocal)
            {
                return GetUshortsFromMemoryDictionary(configurationMemory.LocalMemoryValues, address, numberOfPoints);
            }
            else
            {
                return GetUshortsFromMemoryDictionary(configurationMemory.DeviceMemoryValues, address, numberOfPoints);
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