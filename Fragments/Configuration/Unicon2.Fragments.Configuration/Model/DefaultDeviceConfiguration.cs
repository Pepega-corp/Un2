using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.MemoryAccess;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Name = nameof(DefaultDeviceConfiguration), Namespace = "DefaultDeviceConfigurationNS",
        IsReference = true)]
    public class DefaultDeviceConfiguration : Disposable, IDeviceConfiguration
    {
        private IDataProvider _dataProvider;

        public DefaultDeviceConfiguration()
        {
            this.RootConfigurationItemList = new List<IConfigurationItem>();
        }

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION;

        [DataMember(Name = nameof(RootConfigurationItemList), Order = 1)]

        public List<IConfigurationItem> RootConfigurationItemList { get; set; }

        public bool CheckEquality(IDeviceConfiguration deviceConfigurationToCheck)
        {
            if (deviceConfigurationToCheck.RootConfigurationItemList.Count == this.RootConfigurationItemList.Count)
            {
                foreach (IConfigurationItem configurationItem in deviceConfigurationToCheck.RootConfigurationItemList)
                {
                    if (configurationItem.Name ==
                        this.RootConfigurationItemList[
                            deviceConfigurationToCheck.RootConfigurationItemList.IndexOf(configurationItem)].Name)
                    {
                        if (!this.CheckItemRecursive(configurationItem,
                            this.RootConfigurationItemList[
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

        [DataMember(Name = nameof(ConfigurationSettings), Order = 2)]
        public IFragmentSettings FragmentSettings { get; set; }

        [DataMember(Name = nameof(ConfigurationMemory), Order = 3)]
        public IConfigurationMemory ConfigurationMemory { get; set; }

        public async Task InitializeLocalValues()
        {
            await new ConfigurationMemoryAccessor(_dataProvider, this, ConfigurationMemory, MemoryAccessEnum.InitializeLocalValues).WriteConfigurationMemory();
        }

        public async Task TransferLocalToDeviceValues()
        {
            await new ConfigurationMemoryAccessor(_dataProvider, this, ConfigurationMemory, MemoryAccessEnum.TransferFromLocalToDevice).WriteConfigurationMemory();
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public async Task<bool> Write()
        {
            await new ConfigurationMemoryAccessor(_dataProvider, this, ConfigurationMemory, MemoryAccessEnum.Read).WriteConfigurationMemory();
            return true;
        }

        public async Task Load()
        {
            ConfigurationMemory = await new ConfigurationMemoryAccessor(_dataProvider,this,ConfigurationMemory,MemoryAccessEnum.Read).LoadConfigurationMemory();
        }

        protected override void OnDisposing()
        {
            foreach (IConfigurationItem configurationItem in this.RootConfigurationItemList)
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
                        if (!this.CheckItemRecursive(item,
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