using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Infrastructure.ViewModel;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(IsReference = true, Namespace = "ConfigurationItemBaseNS")]
    public abstract class ConfigurationItemBase : Disposable, IConfigurationItem, IExtensibleDataObject
    {
        private ExtensionDataObject _extensionData;
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        public abstract T Accept<T>(IConfigurationItemVisitor<T> visitor);

        public virtual ExtensionDataObject ExtensionData
        {
            get { return this._extensionData; }
            set { this._extensionData = value; }
        }
        
        public object Clone()
        {
            IConfigurationItem configurationItem = this.OnCloning();
            configurationItem.Description = this.Description;
            configurationItem.Name = this.Name;
            return configurationItem;
        }

        protected abstract IConfigurationItem OnCloning();
        
        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            this.LockObject = new object();
        }
    }

}