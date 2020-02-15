using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings.QuickMemoryAccess;

namespace Unicon2.Model.FragmentSettings
{
    public class QuickMemoryAccessDataProviderStub : IQuickMemoryAccessDataProviderStub
    {
        private readonly IQueryResultFactory _queryResultFactory;
        private IDataProvider _reserveDataProvider;

        public Action TransactionCompleteAction { get; set; }

        public QuickMemoryAccessDataProviderStub(IQueryResultFactory queryResultFactory)
        {
            this._queryResultFactory = queryResultFactory;
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<IQueryResult> WriteMultipleRegistersByBitNumbersAsync(ushort startAddress,
            ushort[] dataToWrite, string dataTitle,
            List<int> bitNumbers)
        {
            IQueryResult queryResult = this._queryResultFactory.CreateDefaultQueryResult();
            if (this.IsMemoryValuesSetsContainsAddress(startAddress))
            {
                bool[] bitArrayToWrite = dataToWrite.GetBoolArrayFromUshortArray();
                bool[] bitArrayExisting =
                    (new[] { this.GetValueFromMemoryValuesSets(startAddress) }).GetBoolArrayFromUshortArray();
                for (int i = 0; i < bitArrayToWrite.Length; i++)
                {
                    if (bitNumbers.Contains(i))
                    {
                        bitArrayExisting[i] = bitArrayToWrite[i];
                    }
                }
                this.SetValueFromMemoryValuesSets(startAddress, bitArrayExisting.BoolArrayToUshort());
            }

            queryResult.IsSuccessful = true;
            return queryResult;
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._reserveDataProvider = dataProvider;
        }

        private bool IsMemoryValuesSetsContainsAddress(ushort address)
        {
            foreach (IMemoryValuesSet memoryValuesSet in this.MemoryValuesSets)
            {
                if (memoryValuesSet.AddressesValuesDictionary.ContainsKey(address)) return true;
            }
            return false;
        }


        private ushort GetValueFromMemoryValuesSets(ushort address)
        {
            foreach (IMemoryValuesSet memoryValuesSet in this.MemoryValuesSets)
            {
                if (memoryValuesSet.AddressesValuesDictionary.ContainsKey(address)) return memoryValuesSet.AddressesValuesDictionary[address];
            }
            return 0;
        }



        private void SetValueFromMemoryValuesSets(ushort address, ushort valueToSet)
        {
            foreach (IMemoryValuesSet memoryValuesSet in this.MemoryValuesSets)
            {
                if (memoryValuesSet.AddressesValuesDictionary.ContainsKey(address)) memoryValuesSet.AddressesValuesDictionary[address] = valueToSet;
            }
        }


        public async Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints, string dataTitle)
        {
            if ((numberOfPoints == 1) && (this.IsMemoryValuesSetsContainsAddress(startAddress)))
            {
                TransactionCompleteAction?.Invoke();
                IQueryResult<ushort[]> queryResult = this._queryResultFactory.CreateDefaultQueryResult<ushort[]>();
                queryResult.IsSuccessful = true;
                queryResult.Result = new ushort[] { this.GetValueFromMemoryValuesSets(startAddress) };
                return queryResult;
            }
            else
            {
                return await this._reserveDataProvider.ReadHoldingResgistersAsync(startAddress, numberOfPoints, dataTitle);
            }
        }

        public async Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            TransactionCompleteAction?.Invoke();
            return await this._reserveDataProvider.ReadCoilStatusAsync(coilAddress, dataTitle);
        }

        public async Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle, ushort numberOfPoints)
        {
            TransactionCompleteAction?.Invoke();
            return await this._reserveDataProvider.ReadCoilStatusAsync(coilAddress, dataTitle, numberOfPoints);
        }

        public async Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite, string dataTitle)
        {
            TransactionCompleteAction?.Invoke();
            if ((dataToWrite.Length == 1) && (this.IsMemoryValuesSetsContainsAddress(startAddress)))
            {
                IQueryResult queryResult = this._queryResultFactory.CreateDefaultQueryResult();
                this.SetValueFromMemoryValuesSets(startAddress, dataToWrite[0]);
                queryResult.IsSuccessful = true;

                return queryResult;
            }
            else
            {
                return await this._reserveDataProvider.WriteMultipleRegistersAsync(startAddress, dataToWrite, dataTitle);

            }
        }

        public async Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            TransactionCompleteAction?.Invoke();
            return await this._reserveDataProvider.WriteSingleCoilAsync(coilAddress, valueToWrite, dataTitle);
        }

        public async Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite, string dataTitle)
        {
            TransactionCompleteAction?.Invoke();
            return await this._reserveDataProvider.WriteSingleRegisterAsync(registerAddress, valueToWrite, dataTitle);
        }

        public bool LastQuerySucceed
        {
            get
            {
                return this._reserveDataProvider.LastQuerySucceed;

            }
        }
    }
}
