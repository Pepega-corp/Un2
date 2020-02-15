using System.Collections.Generic;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess
{
    public interface IQuickMemoryAccessSetting:IFragmentSetting
    {
        List<IRange> QuickAccessAddressRanges { get; set; }
        IQuickMemoryAccessDataProviderStub QuickMemoryAccessDataProviderStub { get; set; }
    }
}