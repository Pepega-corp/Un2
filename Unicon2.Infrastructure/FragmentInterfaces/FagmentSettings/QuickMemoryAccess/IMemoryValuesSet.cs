using System.Collections.Generic;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
    public interface IMemoryValuesSet
    {
        Dictionary<ushort,ushort> AddressesValuesDictionary { get; set; }
    }
}