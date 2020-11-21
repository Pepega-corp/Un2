using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Connections.MockConnection.Keys;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Model.Connection;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.MockConnection.Model
{
    /// <summary>
    /// класс иммитации подключения
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class MockConnection : IDeviceConnection, IDataProvider
    {
        public MockConnection(ITypesContainer typesContainer)
        {
            MemorySlotDictionary = new Dictionary<ushort, ushort>();
            _typesContainer = typesContainer;
        }

        private IDeviceLogger _currentDeviceLogger;
        private ITypesContainer _typesContainer;
        private bool _isConnectionLost;

        public void SetConnectionLost(bool isConnectionLost)
        {
            _isConnectionLost = isConnectionLost;
        }
        
        
        [JsonProperty] public Dictionary<ushort, ushort> MemorySlotDictionary { get; set; }

        public object Clone()
        {
            var o = _typesContainer.Resolve<MockConnection>();
            o.MemorySlotDictionary = MemorySlotDictionary.ToDictionary(entry => entry.Key, entry => entry.Value);
            return o;
        }

        public void Dispose()
        {

        }

        public string ConnectionName => StringKeys.MOCK_CONNECTION;

        public Task<Result> TryOpenConnectionAsync(IDeviceLogger currentDeviceLogger)
        {
            _currentDeviceLogger = currentDeviceLogger;
            return Task.FromResult(Result.Create(true));
        }

        public Action<bool> LastQueryStatusChangedAction { get; set; }

        public void CloseConnection()
        {
        }

        public async Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints,
            string dataTitle)
        {
            if (_isConnectionLost)
            {  
                TransactionCompleteSubscription?.Execute();
                return new DefaultQueryResult<ushort[]>()
                {
                    IsSuccessful = false
                };
            }
            await Task.Delay(2);
            PopulateMemoryIfNeeded(startAddress, numberOfPoints);
            return new DefaultQueryResult<ushort[]>()
            {
                IsSuccessful = true,
                Result = MemorySlotDictionary
                    .Where((pair) => startAddress <= pair.Key && pair.Key <= (startAddress + numberOfPoints - 1))
                    .Select((pair) => pair.Value).ToArray()
            };
        }

        public Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            return Task.FromResult(
                new DefaultQueryResult<bool>() {IsSuccessful = false} as IQueryResult<bool>);
        }

        public Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle,
            ushort numberOfPoints)
        {

            return Task.FromResult(
                new DefaultQueryResult<bool[]>() {IsSuccessful = false} as IQueryResult<bool[]>);
        }

        public async Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite,
            string dataTitle)
        {
            await Task.Delay(2);
            PopulateMemoryIfNeeded(startAddress, (ushort) dataToWrite.Count());
            for (ushort i = startAddress; i < startAddress + (ushort) dataToWrite.Count(); i++)
            {
                MemorySlotDictionary[i] = dataToWrite[i - startAddress];
            }

            return new DefaultQueryResult() {IsSuccessful = true};
        }

        public Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            return Task.FromResult(
                new DefaultQueryResult() {IsSuccessful = false} as IQueryResult);
        }

        public Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite,
            string dataTitle)
        {
            return Task.FromResult(
                new DefaultQueryResult() {IsSuccessful = false} as IQueryResult);
        }

        public IDeviceSubscription TransactionCompleteSubscription { get; set; }

        public bool LastQuerySucceed { get; } = true;

        public bool IsInitialized { get; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            _typesContainer = container;
        }

        private void PopulateMemoryIfNeeded(ushort address, ushort numberOfPoints)
        {
            for (ushort i = address; i < address + numberOfPoints; i++)
            {
                if (!MemorySlotDictionary.ContainsKey(i))
                {
                    MemorySlotDictionary.Add(i, 0);
                }
            }
        }
    }
}