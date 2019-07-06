using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Xml;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Model.DefaultDevice
{
    [DataContract(Namespace = "DefaultDeviceNS", IsReference = true)]
    public class DefaultDevice : Disposable, IDevice
    {
        private ILogService _logService;
        private ISerializerService _serializerService;
        [DataMember]
        private IDeviceConnection _deviceConnection;

        public DefaultDevice(IConnectionState connectionState, ILogService logService,
            ISerializerService serializerService, IDeviceSharedResources deviceSharedResources)
        {
            this.ConnectionState = connectionState;
            this._logService = logService;
            this._serializerService = serializerService;
            this.DeviceFragments = new List<IDeviceFragment>();
            this.DeviceSharedResources = deviceSharedResources;
        }


        [DataMember(Name = nameof(Name))]
        public string Name { get; set; }

        [DataMember(Name = nameof(ConnectionState))]
        public IConnectionState ConnectionState { get; set; }
        [DataMember]
        public IDeviceLogger DeviceLogger { get; set; }
        public IDeviceConnection DeviceConnection
        {
            get { return this._deviceConnection; }
        }
        [DataMember(Name = nameof(DeviceFragments))]
        public IEnumerable<IDeviceFragment> DeviceFragments { get; set; }
        [DataMember(Name = nameof(DeviceSharedResources))]

        public IDeviceSharedResources DeviceSharedResources { get; set; }

        public void SerializeInFile(string elementName, bool isDefaultSaving)
        {
            if (isDefaultSaving)
            {
                if (!(Directory.Exists(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH)))
                {
                    Directory.CreateDirectory(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH);
                }
                elementName = Path.Combine(ApplicationGlobalNames.DEFAULT_DEVICES_FOLDER_PATH, elementName + ".xml");
            }
            try
            {
                using (XmlWriter fs = XmlWriter.Create(elementName, new XmlWriterSettings() { Indent = true }))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(DefaultDevice), this._serializerService.GetTypesForSerialiation());
                    ds.WriteObject(fs, this, this._serializerService.GetNamespacesAttributes());
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public void DeserializeFromFile(string path)
        {
            try
            {
                using (XmlReader fs = XmlReader.Create(path))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(DefaultDevice), this._serializerService.GetTypesForSerialiation());
                    DefaultDevice device = (DefaultDevice)ds.ReadObject(fs);
                    this.Name = device.Name;
                    this.DeviceFragments = device.DeviceFragments;
                    if (device.DeviceSharedResources != null)
                    {
                        this.DeviceSharedResources = device.DeviceSharedResources;
                    }
                    this.ConnectionState = device.ConnectionState;
                    this.DeviceLogger = device.DeviceLogger;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            this.InitializeDeviceFragments();
        }


        [DataMember(Name = nameof(DeviceSignature))]
        public string DeviceSignature { get; set; }


        public void InitializeConnection(IDeviceConnection deviceConnection)
        {
            this._deviceConnection = deviceConnection;
            this._logService.AddLogger(this.DeviceLogger, this.Name);
            this.ConnectionState.Initialize(deviceConnection, this.DeviceLogger);
            if (deviceConnection is IDataProvider)
            {
                foreach (IDeviceFragment fragment in this.DeviceFragments)
                {
                    (fragment as IDataProviderContaining)?.SetDataProvider((IDataProvider)deviceConnection);
                }
            }
        }



        #region Overrides of Disposable

        protected override void OnDisposing()
        {
            this._logService?.DeleteLogger(this.DeviceLogger);
            this._deviceConnection?.Dispose();
            base.OnDisposing();
        }

        #endregion




        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            if (this.IsInitialized) return;

            if (this.ConnectionState == null)
            {
                container.Resolve<IConnectionState>();
            }
            this._logService = container.Resolve<ILogService>();
            this._serializerService = container.Resolve<ISerializerService>();

            (this._deviceConnection as IInitializableFromContainer)?.InitializeFromContainer(container);
            this.DeviceLogger?.InitializeFromContainer(container);

            foreach (IDeviceFragment deviceFragment in this.DeviceFragments)
            {
                (deviceFragment as IInitializableFromContainer)?.InitializeFromContainer(container);
            }
            this.IsInitialized = true;
        }



        private void InitializeDeviceFragments()
        {
            foreach (IDeviceFragment deviceFragment in this.DeviceFragments)
            {
                (deviceFragment as IParentDeviceNameRequirable)?.SetParentDeviceName(this.Name);
            }
        }
        #endregion

    }


}