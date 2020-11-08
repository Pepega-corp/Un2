using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;

namespace Unicon2.Tests.Utils
{
    public static class TestsUtils
    {
        public static Result<IConfigurationItemViewModel> FindItemViewModelByName(this List<IConfigurationItemViewModel> configurationItems, Func<IConfigurationItemViewModel, bool> predicate)
        {
            foreach (var configurationItem in configurationItems)
            {
                if (predicate(configurationItem))
                {
                    return Result<IConfigurationItemViewModel>.Create(configurationItem, true);
                }

                var res = FindItemViewModelByName(configurationItem.ChildStructItemViewModels.ToList(), predicate);
                if (res.IsSuccess)
                {
                    return res;
                }
            }

            return Result<IConfigurationItemViewModel>.Create(false);
        }
        public static Result<IConfigurationItem> FindItemByName(this List<IConfigurationItem> configurationItems, Func<IConfigurationItem, bool> predicate)
        {
            foreach (var configurationItem in configurationItems)
            {
                if (predicate(configurationItem))
                {
                    return Result<IConfigurationItem>.Create(configurationItem, true);
                }

                if (configurationItem is IItemsGroup itemsGroup)
                {
                    var res = FindItemByName(itemsGroup.ConfigurationItemList, predicate);
                    if (res.IsSuccess)
                    {
                        return res;
                    };
                }

                if (configurationItem is IComplexProperty complexProperty)
                {
                    var res = FindItemByName(complexProperty.SubProperties.Cast<IConfigurationItem>().ToList(), predicate);
                    if (res.IsSuccess)
                    {
                        return res;
                    }
                }
            }

            return Result<IConfigurationItem>.Create(false);
        }
    }
}