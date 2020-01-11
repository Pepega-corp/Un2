using System.Collections.Generic;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;

namespace Unicon2.Model.FragmentSettings
{
    public class QuickAccessMemoryApplyingContext : IQuickAccessMemoryApplyingContext
    {

        public QuickAccessMemoryApplyingContext()
        {
            this.DataProviderContainingObjectList=new List<IAddressRangeableWithDataProvider>();
        }


        public QuickAccessModeEnum QuickAccessMode { get; set; }
        public string QueryNameKey { get; set; }
        public List<IAddressRangeableWithDataProvider> DataProviderContainingObjectList { get; set; }
    }
}