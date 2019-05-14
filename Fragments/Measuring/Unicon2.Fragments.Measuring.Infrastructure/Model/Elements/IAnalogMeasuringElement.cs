using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.Elements
{
    public interface IAnalogMeasuringElement:IMeasuringElement,IUshortFormattable,IMeasurable,IInitializableFromContainer, IAddressableItem,ILoadable
    {
        ushort[] DeviceUshortsValue { get; set; }
       new ushort Address { get; set; }
       new ushort NumberOfPoints { get; set; }
    }
}