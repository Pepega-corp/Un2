using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IItemsGroup : IConfigurationItem
    {
        List<IConfigurationItem> ConfigurationItemList { get; set; }
        bool IsTableViewAllowed { get; set; }
        bool? IsMain { get; set; }
        IGroupInfo GroupInfo { get; set; }
        IGroupFilterInfo GroupFilter { get; set; }
    }
}