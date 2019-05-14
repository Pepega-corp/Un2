using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.ModbusMemory.Model
{
    [DataContract(Name = nameof(ModbusMemory), Namespace = "ModbusMemoryNS", IsReference = true)]
    public class ModbusMemory : Disposable, IModbusMemory, IInitializableFromContainer
    {
        private IDataProvider _dataProvider;

        #region Implementation of IModbusMemory

        public List<IModbusMemoryEntity> CurrentValues { get; set; }

        public Action<IDataProvider> DataProviderChanged { get; set; }
        public IDataProvider GetDataProvider()
        {
            return this._dataProvider;
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
            this.DataProviderChanged?.Invoke(dataProvider);
        }

        #endregion

        #region Overrides of Disposable

        protected override void OnDisposing()
        {
            this.DataProviderChanged = null;
            base.OnDisposing();
        }

        #endregion

        #region Implementation of IDeviceFragment

        public IFragmentSettings FragmentSettings { get; set; }

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            this.IsInitialized = true;
            (this.FragmentSettings as IInitializableFromContainer)?.InitializeFromContainer(container);
        }

        #endregion
    }
}
