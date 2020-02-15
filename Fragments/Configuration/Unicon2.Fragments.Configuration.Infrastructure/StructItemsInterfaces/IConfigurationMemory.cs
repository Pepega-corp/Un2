using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IConfigurationMemory
    {
        Dictionary<ushort,ushort> DeviceMemoryValues { get; set; }
        Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
    }
}