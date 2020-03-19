using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.Elements
{
    public interface IDiscretMeasuringElement : IMeasuringElement, IAddressableItem
    {
        IAddressOfBit AddressOfBit { get; set; }
    }
}