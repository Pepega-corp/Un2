using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.Keys;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(Namespace = "DefaultItemsGroupNS", IsReference = true)]
    public class DefaultItemsGroup : ConfigurationItemBase, IItemsGroup
    {
        private ushort _baseAddress;

        public DefaultItemsGroup(Func<IRange> rangeGettingFunc) : base(rangeGettingFunc)
        {
            this.ConfigurationItemList = new List<IConfigurationItem>();
        }

        [DataMember(Name = nameof(ConfigurationItemList), Order = 0)]
        public List<IConfigurationItem> ConfigurationItemList { get; set; }

        [DataMember(Name = nameof(IsTableViewAllowed), Order = 1)]
        public bool IsTableViewAllowed { get; set; }

        [DataMember(Name = nameof(IsMain), Order = 2)]
        public bool? IsMain { get; set; }

        [DataMember(Name = nameof(GroupInfo), Order = 3)]
        public IGroupInfo GroupInfo { get; set; }


        public override string StrongName => ConfigurationKeys.DEFAULT_ITEM_GROUP;

        public override async Task Load()
        {
            if (GroupInfo is IGroupWithReiterationInfo groupWithReiteration && groupWithReiteration.IsReiterationEnabled)
            {
                groupWithReiteration.SetGroupItems(ConfigurationItemList);
                await groupWithReiteration.Load();
            }
            else
            {
                foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
                {
                    await configurationItem.Load();
                }
            }
            await base.Load();
        }


        protected override void FillAddressRanges(List<IRange> ranges)
        {
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                ranges.AddRange(configurationItem.GetAddressesRanges());
            }
        }

        public override async Task<bool> Write()
        {
            bool isWritten = false;
            if (GroupInfo is IGroupWithReiterationInfo groupWithReiteration&&groupWithReiteration.IsReiterationEnabled)
            {
                if (await groupWithReiteration.Write()) isWritten = true;
            }
            else
            {
                foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
                {
                    if (await configurationItem.Write()) isWritten = true;
                }
            }
            await base.Write();
            return isWritten;
        }


        public override void SetDataProvider(IDataProvider dataProvider)
        {
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                configurationItem.SetDataProvider(dataProvider);
            }
            (GroupInfo as IGroupWithReiterationInfo)?.SetDataProvider(dataProvider);
            if (GroupInfo is IGroupWithReiterationInfo groupWithReiteration && groupWithReiteration.IsReiterationEnabled)
            {
                groupWithReiteration.SetGroupItems(ConfigurationItemList);
            }
            base.SetDataProvider(dataProvider);
        }

        protected override void OnDisposing()
        {
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                configurationItem.Dispose();
            }
            base.OnDisposing();
        }



        public override void TransferDeviceLocalData(bool isFromDeviceToLocal)
        {
            if (GroupInfo is IGroupWithReiterationInfo groupWithReiteration && groupWithReiteration.IsReiterationEnabled)
            {
                groupWithReiteration.SubGroups.ForEach((info =>info.ConfigurationItems?.ForEach((item => item.TransferDeviceLocalData(isFromDeviceToLocal))) ));
            }
            else
            {
                foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
                {
                    try
                    {
                        configurationItem.TransferDeviceLocalData(isFromDeviceToLocal);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
            }

            base.TransferDeviceLocalData(isFromDeviceToLocal);
        }



        public override void InitializeLocalValue(IConfigurationItem localConfigurationItem)
        {

            if (!(localConfigurationItem is DefaultItemsGroup)) return;
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                configurationItem.InitializeLocalValue(
                    (localConfigurationItem as DefaultItemsGroup).ConfigurationItemList[
                        this.ConfigurationItemList.IndexOf(configurationItem)]);
            }


            base.InitializeLocalValue(localConfigurationItem);
        }

        public override void InitializeValue(IConfigurationItem localConfigurationItem)
        {
            if (!(localConfigurationItem is DefaultItemsGroup)) return;
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                configurationItem.InitializeValue((localConfigurationItem as DefaultItemsGroup).ConfigurationItemList[this.ConfigurationItemList.IndexOf(configurationItem)]);
            }
            base.InitializeValue(localConfigurationItem);
        }




        protected override IConfigurationItem OnCloning()
        {
            DefaultItemsGroup cloneDefaultItemsGroup = new DefaultItemsGroup(this._rangeGetFunc);
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                cloneDefaultItemsGroup.ConfigurationItemList.Add(configurationItem.Clone() as IConfigurationItem);
            }
            return cloneDefaultItemsGroup;
        }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                configurationItem?.InitializeFromContainer(container);
            }
            base.InitializeFromContainer(container);
        }
    }
}