using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Model.FragmentSettings;

namespace Unicon2.Fragments.Configuration.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultDeviceConfiguration : Disposable, IDeviceConfiguration
    {
        public DefaultDeviceConfiguration()
        {
            RootConfigurationItemList = new List<IConfigurationItem>();
            FragmentSettings = new DefaultFragmentSettings();
        }

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION;
        [JsonProperty] public List<IConfigurationItem> RootConfigurationItemList { get; set; }

        public bool CheckEquality(IDeviceConfiguration deviceConfigurationToCheck)
        {
            if (deviceConfigurationToCheck.RootConfigurationItemList.Count == RootConfigurationItemList.Count)
            {
                foreach (IConfigurationItem configurationItem in deviceConfigurationToCheck.RootConfigurationItemList)
                {
                    if (configurationItem.Name ==
                        RootConfigurationItemList[
                            deviceConfigurationToCheck.RootConfigurationItemList.IndexOf(configurationItem)].Name)
                    {
                        if (!CheckItemRecursive(configurationItem,
                            RootConfigurationItemList[
                                deviceConfigurationToCheck.RootConfigurationItemList.IndexOf(configurationItem)]))
                            return false;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        [JsonProperty] public IConfigurationBaseValues BaseValues { get; set; }

        [JsonProperty] public IFragmentSettings FragmentSettings { get; set; }

        protected override void OnDisposing()
        {
            foreach (IConfigurationItem configurationItem in RootConfigurationItemList)
            {
                configurationItem.Dispose();
            }

            base.OnDisposing();
        }

        private bool CheckItemRecursive(IConfigurationItem configurationItem,
            IConfigurationItem configurationItemToCheck)
        {
            if ((configurationItem is IItemsGroup) && (configurationItemToCheck is IItemsGroup))
            {
                if ((configurationItem.Name == configurationItemToCheck.Name) &&
                    ((IItemsGroup) configurationItem).ConfigurationItemList.Count ==
                    ((IItemsGroup) configurationItemToCheck).ConfigurationItemList.Count)
                {
                    foreach (IConfigurationItem item in (configurationItem as IItemsGroup)
                        .ConfigurationItemList)
                    {
                        if (!CheckItemRecursive(item,
                            ((IItemsGroup) configurationItemToCheck).ConfigurationItemList[
                                ((IItemsGroup) configurationItem).ConfigurationItemList.IndexOf(item)])) return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return configurationItem.Name == configurationItemToCheck.Name;
            }

            return true;
        }
    }
}