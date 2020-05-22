using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime;

namespace Unicon2.Fragments.Configuration.Infrastructure.Export
{
    public class SelectorForItemsGroup
    {
        public SelectorForItemsGroup(IEnumerable<SelectorForItemsGroup> selectors, IRuntimeItemGroupViewModel relatedItemsGroup,
            bool isSelected, bool isPrintDeviceValuesAllowed, bool isPrintLocalValuesAllowed)
        {
            Selectors = selectors;
            RelatedItemsGroup = relatedItemsGroup;
            IsSelected = isSelected;
            IsPrintDeviceValuesAllowed = isPrintDeviceValuesAllowed;
            IsPrintLocalValuesAllowed = isPrintLocalValuesAllowed;
        }

        public bool IsSelected { get; }
        public IEnumerable<SelectorForItemsGroup> Selectors { get; }

        public IRuntimeItemGroupViewModel RelatedItemsGroup { get; }
        public bool IsPrintDeviceValuesAllowed { get; }
        public bool IsPrintLocalValuesAllowed { get; }
    }
}