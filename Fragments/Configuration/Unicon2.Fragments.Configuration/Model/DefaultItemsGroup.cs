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
        public DefaultItemsGroup() 
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
        
        protected override void OnDisposing()
        {
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                configurationItem.Dispose();
            }
            base.OnDisposing();
        }

        protected override IConfigurationItem OnCloning()
        {
            DefaultItemsGroup cloneDefaultItemsGroup = new DefaultItemsGroup();
            foreach (IConfigurationItem configurationItem in this.ConfigurationItemList)
            {
                cloneDefaultItemsGroup.ConfigurationItemList.Add(configurationItem.Clone() as IConfigurationItem);
            }
            cloneDefaultItemsGroup.GroupInfo
            return cloneDefaultItemsGroup;
        }
        
    }
}