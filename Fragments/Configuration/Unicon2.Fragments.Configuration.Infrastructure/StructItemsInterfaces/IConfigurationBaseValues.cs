using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IConfigurationBaseValues
    {
        List<IConfigurationBaseValue> BaseValues { get; set; }
    }

    public interface IConfigurationBaseValue
    {
        string Name { get; set; }
        Dictionary<ushort, ushort> LocalMemoryValues { get; set; }
    }
}