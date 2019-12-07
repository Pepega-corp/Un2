using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.Export
{
    public class SelectorForItemsGroup
    {
        public SelectorForItemsGroup(IEnumerable<SelectorForItemsGroup> selectors, IItemsGroup relatedItemsGroup,
            bool isSelected)
        {
            Selectors = selectors;
            RelatedItemsGroup = relatedItemsGroup;
            IsSelected = isSelected;
        }

        public bool IsSelected { get; }
        public IEnumerable<SelectorForItemsGroup> Selectors { get; }

        public IItemsGroup RelatedItemsGroup { get; }

    }
}