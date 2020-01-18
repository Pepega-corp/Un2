using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces
{
    public interface IItemsGroup : IConfigurationItem
    {
        List<IConfigurationItem> ConfigurationItemList { get; set; }
        bool IsTableViewAllowed { get; set; }
    }
}