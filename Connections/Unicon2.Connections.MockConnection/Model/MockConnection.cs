using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Model.Connection;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.MockConnection.Model
{ 
    /// <summary>
    /// класс иммитации подключения
    /// </summary>
    [DataContract(Namespace = "ModBusRtuConnectionNS", IsReference = true)]
    public class MockConnection : IDeviceConnection, IDataProvider, IInitializableFromContainer
    {
        public MockConnection()
        {
            MemorySlots=new Dictionary<int, ushort>();
        }

        private IDeviceLogger _currentDeviceLogger;

        [DataMember]
        private Dictionary<int, ushort> MemorySlots { get; }


        #region Implementation of ICloneable

        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Implementation of IDeviceConnection

        public string ConnectionName { get; }
        public Task<bool> TryOpenConnectionAsync(bool isThrowingException, IDeviceLogger currentDeviceLogger)
        {
            _currentDeviceLogger = currentDeviceLogger;
            return Task.FromResult(true);
        }

        public Action<bool> LastQueryStatusChangedAction { get; set; }
        public void CloseConnection()
        {
        }

        #endregion

        #region Implementation of IDataProvider

        public async Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints, string dataTitle)
        {
            throw new NotImplementedException();
            //   return new DefaultQueryResult<ushort[]>(){IsSuccessful=true,Result = MemorySlots.Select()};
        }

        public Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle, ushort numberOfPoints)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite, string dataTitle)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite, string dataTitle)
        {
            throw new NotImplementedException();
        }

        public bool LastQuerySucceed { get; }

        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized { get; }
        public void InitializeFromContainer(ITypesContainer container)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
