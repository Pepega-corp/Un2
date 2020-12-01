using System;
using System.Collections.Generic;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.Configuration.ViewModelMemoryMapping
{
    public static class MemoryAccessor
    {

	    public static void ClearRangeTo(ushort rangeFrom, ushort rangeTo, Dictionary<ushort, ushort> memorySet)
	    {
		    for (var i = rangeFrom; i <= rangeTo; i++)
		    {
			    memorySet.Remove(i);
		    }
	    }

	    public static void ClearRange(ushort rangeFrom, ushort numberOfPoints, Dictionary<ushort, ushort> memorySet)
	    {
		    var rangeTo = rangeFrom + numberOfPoints;
		    for (var i = rangeFrom; i <= rangeTo; i++)
		    {
			    memorySet.Remove(i);
		    }
	    }

	    public static bool IsMemoryContainsAddresses(IDeviceMemory deviceMemory, ushort address,
		    ushort numberOfPoints, bool isLocal)
	    {
		    if (isLocal)
		    {
			    return IsMemoryDictionaryContainsAddresses(deviceMemory.LocalMemoryValues, address, numberOfPoints);
		    }
		    else
		    {
			    return IsMemoryDictionaryContainsAddresses(deviceMemory.DeviceMemoryValues, address, numberOfPoints);
		    }
		}

        public static Result<ushort[]> GetUshortsFromMemorySafe(IDeviceMemory deviceMemory, ushort address,
            ushort numberOfPoints, bool isLocal)
        {
            if (!IsMemoryContainsAddresses(deviceMemory, address, numberOfPoints, isLocal))
            {
                return Result<ushort[]>.Create(false);
            }
                if (isLocal)
            {
                
                return Result<ushort[]>.Create(GetUshortsFromMemoryDictionary(deviceMemory.LocalMemoryValues, address, numberOfPoints),true);
            }
            else
            {
                return Result<ushort[]>.Create(GetUshortsFromMemoryDictionary(deviceMemory.DeviceMemoryValues, address, numberOfPoints), true);
            }
        }
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

        public static void SetUshortsInMemory(IDeviceMemory deviceMemory, ushort address,
            ushort[] values, bool isLocal)
        {
            if(deviceMemory==null)return;
            if (isLocal)
            {
                SetUshortsInMemoryDictionary(deviceMemory.LocalMemoryValues, address, values);
            }
            else
            {
                SetUshortsInMemoryDictionary(deviceMemory.DeviceMemoryValues, address, values);
            }
        }

        private static void SetUshortsInMemoryDictionary(
            Dictionary<ushort, ushort> configurationMemoryLocalMemoryValues, ushort address, ushort[] values)
        {
            ushort addressCurrent = address;
            foreach (var value in values)
            {
                if (configurationMemoryLocalMemoryValues.ContainsKey(addressCurrent))
                {
                    configurationMemoryLocalMemoryValues[addressCurrent] = value;
                }
                else
                {
                    configurationMemoryLocalMemoryValues.Add(addressCurrent, value);
                }

                addressCurrent++;
            }
        }

        private static bool IsMemoryDictionaryContainsAddresses(Dictionary<ushort, ushort> configurationMemoryValues,
	        ushort address, ushort numberOfPoints)
        {
	        for (var i = address; i < address + numberOfPoints; i++)
	        {
		        if (!configurationMemoryValues.ContainsKey(i))
		        {
			        return false;
		        }
	        }
	        return true;
        }
		private static ushort[] GetUshortsFromMemoryDictionary(Dictionary<ushort, ushort> configurationMemoryValues,
            ushort address, ushort numberOfPoints)
        {
            var result = new ushort[numberOfPoints];
            var counter = 0;
            for (var i = address; i < address + numberOfPoints; i++)
            {
	            try
	            {
		            result[counter++] = configurationMemoryValues[i];
				}
	            catch (Exception e)
	            {
		            Console.WriteLine(e);
		            throw;
	            }
                
            }

            return result;
        }



    }
}