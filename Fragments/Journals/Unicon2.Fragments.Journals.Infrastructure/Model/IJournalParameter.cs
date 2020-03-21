using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Infrastructure.Model
{
    public interface IJournalParameter : IUshortFormattable, IMeasurable
    {
        ushort StartAddress { get; set; }
        ushort NumberOfPoints { get; set; }
    }
}