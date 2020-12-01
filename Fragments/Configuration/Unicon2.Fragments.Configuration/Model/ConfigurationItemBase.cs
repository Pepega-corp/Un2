using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.BaseItems;

namespace Unicon2.Fragments.Configuration.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ConfigurationItemBase : Disposable, IConfigurationItem, IExtensibleDataObject
    {
        private ExtensionDataObject _extensionData;
        
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string Description { get; set; }

        public abstract T Accept<T>(IConfigurationItemVisitor<T> visitor);

        public virtual ExtensionDataObject ExtensionData
        {
            get { return _extensionData; }
            set { _extensionData = value; }
        }
        
        public object Clone()
        {
            IConfigurationItem configurationItem = OnCloning();
            configurationItem.Description = Description;
            configurationItem.Name = Name;
            return configurationItem;
        }

        protected abstract IConfigurationItem OnCloning();
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            LockObject = new object();
        }
    }

}