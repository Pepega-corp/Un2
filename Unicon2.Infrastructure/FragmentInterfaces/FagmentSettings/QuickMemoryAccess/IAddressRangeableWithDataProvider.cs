using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
    public interface IAddressRangeableWithDataProvider:IDataProviderContaining
    {
        List<IRange> GetAddressesRanges();
    }
}