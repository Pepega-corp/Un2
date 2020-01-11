using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.FragmentSettings
{
    public class MemoryValueSet : IMemoryValuesSet
    {
        public MemoryValueSet(IRange range, ushort[] ushorts)
        {
            int index = 0;
            this.AddressesValuesDictionary = new Dictionary<ushort, ushort>();
            if (ushorts == null) return;
            foreach (var value in ushorts)
            {
                this.AddressesValuesDictionary.Add((ushort)((ushort)range.RangeFrom + (ushort)index), value);
                index++;
            }
        }

        public Dictionary<ushort, ushort> AddressesValuesDictionary { get; set; }
    }
}
