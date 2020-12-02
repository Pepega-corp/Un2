using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Model.Connection;

namespace Unicon2.Tests.Helpers.Query
{
    public class MockConnectionWithSetup : IDeviceConnection, IDataProvider
    {
        private readonly List<(bool wasHit,QueryMockDefinition definition)> _queryMockDefinitions;

        public MockConnectionWithSetup(List<QueryMockDefinition> queryMockDefinitions)
        {
            _queryMockDefinitions = queryMockDefinitions.Select(definition => (false, definition)).ToList();
        }



        public object Clone()
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public string ConnectionName { get; }
        public Task<Result> TryOpenConnectionAsync(IDeviceLogger currentDeviceLogger)
        {
            return Task.FromResult<Result>(true);
        }

        public void CloseConnection()
        {
            
        }

        public async Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints, string dataTitle)
        {
            var definition = _queryMockDefinitions.First(tuple => !tuple.wasHit && tuple.definition.FuncNumber == 3);
            if (definition.definition.Address == startAddress && definition.definition.NumberOfPoints == numberOfPoints)
            {      
                definition.wasHit = true;
                return new DefaultQueryResult<ushort[]>()
                {
                    IsSuccessful = true,
                    Result = definition.definition.Data.ToArray()
                    
                };
            }
            else
            {
                return new DefaultQueryResult<ushort[]>()
                {
                    IsSuccessful = false
                };
            }
        }

        public Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            throw new System.NotImplementedException();
        }

        public Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle, ushort numberOfPoints)
        {
            throw new System.NotImplementedException();
        }

        public Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite, string dataTitle)
        {
            throw new System.NotImplementedException();
        }

        public Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite, string dataTitle)
        {
            var definition = _queryMockDefinitions.First(tuple => !tuple.wasHit && tuple.definition.FuncNumber == 6);
            if (definition.definition.Address == registerAddress && definition.definition.Data[0] == valueToWrite)
            {
                definition.wasHit = true;
                return new DefaultQueryResult()
                {
                    IsSuccessful = true
                };
            }
            else
            {
                return new DefaultQueryResult()
                {
                    IsSuccessful = false
                };
            }
        }

        public IDeviceSubscription TransactionCompleteSubscription { get; set; }
        public bool LastQuerySucceed { get; }
    }
}