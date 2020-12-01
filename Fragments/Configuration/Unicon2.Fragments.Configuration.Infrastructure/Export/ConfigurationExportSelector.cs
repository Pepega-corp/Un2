using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.Export
{
    public class ConfigurationExportSelector
    {
        public ConfigurationExportSelector(bool isPrintDeviceValuesAllowed, bool isPrintLocalValuesAllowed, List<SelectorForItemsGroup> selectorForItemsGroup)
        {
            IsPrintDeviceValuesAllowed = isPrintDeviceValuesAllowed;
            IsPrintLocalValuesAllowed = isPrintLocalValuesAllowed;
            SelectorForItemsGroup = selectorForItemsGroup;
        }

        public List<SelectorForItemsGroup> SelectorForItemsGroup { get; }
        public bool IsPrintDeviceValuesAllowed { get; }
        public bool IsPrintLocalValuesAllowed { get; }
    }
}
