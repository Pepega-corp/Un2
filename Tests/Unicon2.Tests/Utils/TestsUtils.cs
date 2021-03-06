﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Properties;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.TreeGrid;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;

namespace Unicon2.Tests.Utils
{
    public static class TestsUtils
    {
        public static List<IConfigurationItemViewModel> FindAllItemViewModelsByName(
            this List<IConfigurationItemViewModel> configurationItems,
            Func<IConfigurationItemViewModel, bool> predicate,
            List<IConfigurationItemViewModel> result =null)
        {
            if (result == null)
            {
                result=new List<IConfigurationItemViewModel>();
            }
            foreach (var configurationItem in configurationItems)
            {
                if (predicate(configurationItem))
                {
                    result.Add(configurationItem);
                }
                FindAllItemViewModelsByName(configurationItem.ChildStructItemViewModels.ToList(), predicate, result);
            }

            return result;
        }

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

        public static async Task<bool> WaitUntil(Func<bool> predicate, int millisecondsToWait=10000, int interval=50)
        {
            int iterations = millisecondsToWait / interval;
            bool result = false;
            for (int i = 0; i < iterations; i++)
            {
                await Task.Delay(interval);
                if (predicate())
                {
                    result = true;
                    break;
                }
            }
            return result;
        }
    }
}