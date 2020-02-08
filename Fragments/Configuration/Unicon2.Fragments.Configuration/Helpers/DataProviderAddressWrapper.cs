using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Configuration.Helpers
{
    public class DataProviderAddressWrapper:IDataProvider
    {
        private readonly IDataProvider _dataProvider;
        private readonly ushort _addressOffset;

        public DataProviderAddressWrapper(IDataProvider dataProvider, ushort addressOffset)
        {
            _dataProvider = dataProvider;
            _addressOffset = addressOffset;
        }
        public void Dispose()
        {
            _dataProvider.Dispose();
        }

        public Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints,
            string dataTitle)
        {
           return _dataProvider.ReadHoldingResgistersAsync((ushort) (startAddress + _addressOffset), numberOfPoints,
                dataTitle);
        }

        public Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            return _dataProvider.ReadCoilStatusAsync((ushort)(coilAddress + _addressOffset), dataTitle);
        }

        public Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle, ushort numberOfPoints)
        {
            return _dataProvider.ReadCoilStatusAsync((ushort)(coilAddress + _addressOffset), dataTitle,numberOfPoints);
        }

        public Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite, string dataTitle)
        {
            return _dataProvider.WriteMultipleRegistersAsync((ushort)(startAddress + _addressOffset), dataToWrite, dataTitle);
        }

        public Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            return _dataProvider.WriteSingleCoilAsync((ushort)(coilAddress + _addressOffset), valueToWrite, dataTitle);
        }

        public Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite, string dataTitle)
        {
            return _dataProvider.WriteSingleRegisterAsync((ushort)(registerAddress + _addressOffset), valueToWrite, dataTitle);
        }

        public Action TransactionCompleteAction
        {
            get => _dataProvider.TransactionCompleteAction;
            set => _dataProvider.TransactionCompleteAction = value;
        }

        public bool LastQuerySucceed => _dataProvider.LastQuerySucceed;
    }
}
