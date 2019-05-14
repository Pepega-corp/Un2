using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Model.Base;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Configuration.Model
{
    [DataContract(IsReference = true, Namespace = "ConfigurationItemBaseNS")]
    [KnownType(typeof(LocalDeviceValuesConfigurationItemBase))]
    public abstract class ConfigurationItemBase : Disposable, IConfigurationItem, IExtensibleDataObject
    {
        private ExtensionDataObject _extensionData;
        protected IDataProvider _dataProvider;
        protected IDeviceSharedResources _deviceSharedResources;
        private List<Guid> _relatedResourceGuidList;
        protected Func<IRange> _rangeGetFunc;

        protected ConfigurationItemBase(Func<IRange> rangeGetFunc)
        {
            this._rangeGetFunc = rangeGetFunc;
        }


        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Description { get; set; }

        public virtual void TransferDeviceLocalData(bool isFromDeviceToLocal)
        {
            this.ConfigurationItemChangedAction?.Invoke();
        }

        public virtual void InitializeLocalValue(IConfigurationItem localConfigurationItem)
        {
            this.ConfigurationItemChangedAction?.Invoke();
        }

        public Action ConfigurationItemChangedAction { get; set; }

        public virtual ExtensionDataObject ExtensionData
        {
            get { return this._extensionData; }
            set { this._extensionData = value; }
        }

        #region Implementation of IStronglyNamed

        public abstract string StrongName { get; }

        #endregion

        #region Overrides of Disposable

        protected override void OnDisposing()
        {
            this.ConfigurationItemChangedAction = null;
            base.OnDisposing();
        }

        #endregion

        #region Implementation of IDataProviderContaining

        public virtual void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public List<IRange> GetAddressesRanges()
        {
            List<IRange> ranges = new List<IRange>();
            this.FillAddressRanges(ranges);
            return ranges;
        }

        protected abstract void FillAddressRanges(List<IRange> ranges);

        public virtual async Task<bool> Write()
        {
            return true;
        }

        public virtual async Task Load()
        {
            this.ConfigurationItemChangedAction?.Invoke();
        }

        #endregion


        #region Implementation of ICloneable

        public object Clone()
        {
            IConfigurationItem configurationItem = this.OnCloning();
            configurationItem.Description = this.Description;
            configurationItem.Name = this.Name;

            return configurationItem;
        }

        protected abstract IConfigurationItem OnCloning();

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public virtual void InitializeFromContainer(ITypesContainer container)
        {
            this.IsInitialized = true;
            this._rangeGetFunc = container.Resolve<Func<IRange>>();
        }

        #endregion

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            this.LockObject = new object();
        }
    }

}