using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Journals.Infrastructure.Model
{
    public interface IJournalParameter : IUshortFormattable, IMeasurable, IInitializableFromContainer
    {
        ushort StartAddress { get; set; }
        ushort NumberOfPoints { get; set; }
        Task<List<IFormattedValue>> GetFormattedValues(ushort[] recordUshorts);

    }
}