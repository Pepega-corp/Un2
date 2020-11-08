using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Filters;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Configuration.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultItemsGroup : ConfigurationItemBase, IItemsGroup
    {
        public DefaultItemsGroup() 
        {
            ConfigurationItemList = new List<IConfigurationItem>();
        }

        [JsonProperty]
        public List<IConfigurationItem> ConfigurationItemList { get; set; }

        [JsonProperty]
        public bool IsTableViewAllowed { get; set; }

        [JsonProperty]
        public bool? IsMain { get; set; }

        [JsonProperty]
        public IGroupInfo GroupInfo { get; set; }

        [JsonProperty]
        public IGroupFilterInfo GroupFilter { get; set; }

        public override T Accept<T>(IConfigurationItemVisitor<T> visitor)
        {
            return visitor.VisitItemsGroup(this);
        }
        protected override void OnDisposing()
        {
            foreach (IConfigurationItem configurationItem in ConfigurationItemList)
            {
                configurationItem.Dispose();
            }
            base.OnDisposing();
        }
        protected override IConfigurationItem OnCloning()
        {
            DefaultItemsGroup cloneDefaultItemsGroup = new DefaultItemsGroup();
            foreach (IConfigurationItem configurationItem in ConfigurationItemList)
            {
                cloneDefaultItemsGroup.ConfigurationItemList.Add(configurationItem.Clone() as IConfigurationItem);
            }
            cloneDefaultItemsGroup.GroupInfo=GroupInfo.Clone() as IGroupInfo;
            return cloneDefaultItemsGroup;
        }
       

    }
}