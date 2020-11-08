using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;

namespace Unicon2.Fragments.Measuring.Infrastructure.Model.Elements
{
    public interface IDiscretMeasuringElement : IMeasuringElement, IAddressableItem
    {
        IAddressOfBit AddressOfBit { get; set; }
    }
}