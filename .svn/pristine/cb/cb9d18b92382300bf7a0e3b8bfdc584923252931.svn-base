using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Name = nameof(DefaultDeviceConfiguration), Namespace = "DefaultDeviceConfigurationNS", IsReference = true)]
    public class DefaultDeviceConfiguration : Disposable, IDeviceConfiguration
    {
        private ISerializerService _serializerService;
        private IDataProvider _dataProvider;
        private Func<IQuickAccessMemoryApplyingContext> _quickAccessMemoryApplyingContextFunc;
        public DefaultDeviceConfiguration(ISerializerService serializerService, Func<IQuickAccessMemoryApplyingContext> quickAccessMemoryApplyingContextFunc)
        {
            this._serializerService = serializerService;
            this._quickAccessMemoryApplyingContextFunc = quickAccessMemoryApplyingContextFunc;
            this.RootConfigurationItemList = new List<IConfigurationItem>();
        }

        #region Implementation of IStronglyNamed

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.CONFIGURATION;

        #endregion
        
        #region Implementation of IDeviceConfiguration

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

        #endregion

        #region Implementation of ISerializableInFile

        public void SerializeInFile(string elementName, bool isDefaultSaving)
        {
            try
            {
                using (XmlWriter fs = XmlWriter.Create(elementName, new XmlWriterSettings { Indent = true }))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(DefaultDeviceConfiguration),
                        this._serializerService.GetTypesForSerialiation());

                    ds.WriteObject(fs, this);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeserializeFromFile(string path)
        {
            try
            {
                using (XmlReader fs = XmlReader.Create(path))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(DefaultDeviceConfiguration),
                        this._serializerService.GetTypesForSerialiation());
                    this.RootConfigurationItemList = ((DefaultDeviceConfiguration)ds.ReadObject(fs)).RootConfigurationItemList;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new SerializationException();
            }
        }

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (this.IsInitialized) return;
            foreach (IConfigurationItem root in this.RootConfigurationItemList)
            {
                root?.InitializeFromContainer(container);
            }
           (this.FragmentSettings as IInitializableFromContainer)?.InitializeFromContainer(container);
            this._quickAccessMemoryApplyingContextFunc = container.Resolve<Func<IQuickAccessMemoryApplyingContext>>();
            this._serializerService = container.Resolve<ISerializerService>();
            this.IsInitialized = true;
        }

        #endregion

        #region Implementation of IDataProviderContaining

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
            foreach (IConfigurationItem rootConfigurationItem in this.RootConfigurationItemList)
            {
                rootConfigurationItem.SetDataProvider(dataProvider);
            }
            if (this.FragmentSettings != null)
            {
                foreach (IFragmentSetting configurationSetting in this.FragmentSettings.FragmentSettings)
                {
                    if (configurationSetting is IDataProviderContaining)
                    {
                        (configurationSetting as IDataProviderContaining).SetDataProvider(dataProvider);
                    }
                }
            }
        }

        public async Task<bool> Write()
        {
            bool isWritten = false;
            if (this._dataProvider == null) return false;
            foreach (IConfigurationItem configurationItem in this.RootConfigurationItemList)
            {
                if (await configurationItem.Write()) isWritten = true;
            }
            if (isWritten)
            {
                IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext = this._quickAccessMemoryApplyingContextFunc();
                quickAccessMemoryApplyingContext.QueryNameKey = ConfigurationKeys.WRITING_CONFIGURATION_QUERY;
                foreach (IConfigurationItem rootConfigurationItem in this.RootConfigurationItemList)
                {
                    quickAccessMemoryApplyingContext.DataProviderContainingObjectList.Add(rootConfigurationItem);
                }
                quickAccessMemoryApplyingContext.QuickAccessMode = QuickAccessModeEnum.Write;
                Task<bool> applySettingByKey = this.FragmentSettings?.ApplySettingByKey(ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING, quickAccessMemoryApplyingContext);
                bool isSettingApplied = applySettingByKey != null && await applySettingByKey;
                if (isSettingApplied)
                {
                    foreach (IConfigurationItem rootConfigurationItem in this.RootConfigurationItemList)
                    {
                        rootConfigurationItem.TransferDeviceLocalData(false);
                    }
                }
            }


            return isWritten;
        }

        public async Task Load()
        {
            await this.LoadAsync();

            foreach (IConfigurationItem rootConfigurationItem in this.RootConfigurationItemList)
            {
                await rootConfigurationItem.Load();
            }
        }

        private async Task LoadAsync()
        {
            IQuickAccessMemoryApplyingContext quickAccessMemoryApplyingContext = this._quickAccessMemoryApplyingContextFunc();
            quickAccessMemoryApplyingContext.QueryNameKey = ConfigurationKeys.READING_CONFIGURATION_QUERY;
            foreach (IConfigurationItem rootConfigurationItem in this.RootConfigurationItemList)
            {
                quickAccessMemoryApplyingContext.DataProviderContainingObjectList.Add(rootConfigurationItem);
            }
            quickAccessMemoryApplyingContext.QuickAccessMode = QuickAccessModeEnum.Initialize;

            Task applySettingByKey = this.FragmentSettings?.ApplySettingByKey(
                ApplicationGlobalNames.QUICK_ACCESS_MEMORY_CONFIGURATION_SETTING,
                quickAccessMemoryApplyingContext);
            if (applySettingByKey != null)
                await applySettingByKey;
        }


        #region Overrides of Disposable

        protected override void OnDisposing()
        {
            foreach (IConfigurationItem configurationItem in this.RootConfigurationItemList)
            {
                configurationItem.Dispose();
            }
            base.OnDisposing();
        }

        #endregion

        #endregion


        #region Helpers

        private bool CheckItemRecursive(IConfigurationItem configurationItem,
            IConfigurationItem configurationItemToCheck)
        {
            if ((configurationItem is IItemsGroup) && (configurationItemToCheck is IItemsGroup))
            {
                if ((configurationItem.Name == configurationItemToCheck.Name) &&
                    ((IItemsGroup)configurationItem).ConfigurationItemList.Count ==
                    ((IItemsGroup)configurationItemToCheck).ConfigurationItemList.Count)
                {
                    foreach (IConfigurationItem item in (configurationItem as IItemsGroup)
                        .ConfigurationItemList)
                    {
                        if (!this.CheckItemRecursive(item,
                            ((IItemsGroup)configurationItemToCheck).ConfigurationItemList[
                                ((IItemsGroup)configurationItem).ConfigurationItemList.IndexOf(item)])) return false;

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


        #endregion


    }
}