using NModbus4.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Threading;
using Newtonsoft.Json;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.DataProvider.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class ModbusDataProvider : Disposable, IDataProvider
    {
        private IQueryResultFactory _queryResultFactory;
        protected bool _isInitialized;
        protected IModbusMaster _currentModbusMaster;
        protected byte _slaveId = 0; //for debug number of device =1. Defaul 0
        protected bool _lastQuerySucceed;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1, 1);

        public Action TransactionCompleteAction { get; set; }

        protected virtual bool CheckConnection(IQueryResult queryResult)
        {
            if (_currentModbusMaster.Transport == null)
            {
                queryResult.IsSuccessful = false;
                return false;
            }

            return true;
        }

        protected ModbusDataProvider(IQueryResultFactory queryResultFactory)
        {
            _queryResultFactory = queryResultFactory;
        }


        public async Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints,
            string dataTitle)
        {
            await _semaphoreSlim.WaitAsync();
            IQueryResult<ushort[]> queryResult = _queryResultFactory.CreateDefaultQueryResult<ushort[]>();
            if (!CheckConnection(queryResult)) return queryResult;
            try
            {
                TransactionCompleteAction?.Invoke();
                queryResult.Result =
                    await _currentModbusMaster.ReadHoldingRegistersAsync(_slaveId, startAddress, numberOfPoints);
                List<string> results = queryResult.Result.Select((arg => arg.ToString())).ToList();
                string resStr = "";
                foreach (string res in results)
                {
                    resStr += res;
                    resStr += " ";
                }

                LogQuery(true, dataTitle,
                    "Fun:3" + " Addr:" + startAddress + " Num:" + numberOfPoints + " Data:" + resStr);
                queryResult.IsSuccessful = true;

            }
            catch (Exception e)
            {
                LogQuery(false, dataTitle, "Fun:3" + " Addr:" + startAddress + " Num:" + numberOfPoints, exception: e);
                queryResult.IsSuccessful = false;
            }

            _semaphoreSlim.Release(1);
            return queryResult;
        }

        public async Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            await _semaphoreSlim.WaitAsync();
            IQueryResult<bool> queryResult = _queryResultFactory.CreateDefaultQueryResult<bool>();
            if (!CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                queryResult.Result = (await _currentModbusMaster.ReadCoilsAsync(_slaveId, coilAddress, 1))[0];
                LogQuery(true, dataTitle,
                    "Fun:1" + " Addr:" + coilAddress + " Num:" + 1 + " Data:" + queryResult.Result);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                LogQuery(false, dataTitle, "Fun:1" + " Addr:" + coilAddress + " Num:" + 1, exception: e);
                queryResult.IsSuccessful = false;
            }

            _semaphoreSlim.Release(1);
            return queryResult;
        }

        public async Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle,
            ushort numberOfPoints)
        {
            await _semaphoreSlim.WaitAsync();
            IQueryResult<bool[]> queryResult = _queryResultFactory.CreateDefaultQueryResult<bool[]>();
            if (!CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                queryResult.Result = await _currentModbusMaster.ReadCoilsAsync(_slaveId, coilAddress, numberOfPoints);
                string resStr = "";
                foreach (bool res in queryResult.Result)
                {
                    resStr += res;
                    resStr += " ";
                }

                LogQuery(true, dataTitle,
                    "Fun:1" + " Addr:" + coilAddress + " Num:" + numberOfPoints + " Data:" + resStr);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                LogQuery(false, dataTitle, "Fun:1" + " Addr:" + coilAddress + " Num:" + numberOfPoints, exception: e);
                queryResult.IsSuccessful = false;
            }

            _semaphoreSlim.Release(1);
            return queryResult;
        }

        public async Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite,
            string dataTitle)
        {
            await _semaphoreSlim.WaitAsync();
            IQueryResult queryResult = _queryResultFactory.CreateDefaultQueryResult();
            if (!CheckConnection(queryResult)) return queryResult;

            string dataStr = "";
            foreach (ushort res in dataToWrite)
            {
                dataStr += res;
                dataStr += " ";
            }

            try
            {
                TransactionCompleteAction?.Invoke();
                await _currentModbusMaster.WriteMultipleRegistersAsync(_slaveId, startAddress, dataToWrite);

                LogQuery(true, dataTitle, "Fun:16" + " Addr:" + startAddress + " Data:" + dataStr);

                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                LogQuery(false, dataTitle, "Fun:16" + " Addr:" + startAddress + " Data:" + dataStr, exception: e);
                queryResult.IsSuccessful = false;

            }

            _semaphoreSlim.Release(1);
            return queryResult;
        }



        public async Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            await _semaphoreSlim.WaitAsync();
            IQueryResult queryResult = _queryResultFactory.CreateDefaultQueryResult();
            if (!CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                await _currentModbusMaster.WriteSingleCoilAsync(_slaveId, coilAddress, valueToWrite);
                LogQuery(true, dataTitle, "Fun:5" + " Addr:" + coilAddress + " Data:" + valueToWrite);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                LogQuery(false, dataTitle, "Fun:5" + " Addr:" + coilAddress + " Data:" + valueToWrite, exception: e);
                queryResult.IsSuccessful = false;
            }

            _semaphoreSlim.Release(1);
            return queryResult;
        }

        public async Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite,
            string dataTitle)
        {
            await _semaphoreSlim.WaitAsync();
            IQueryResult queryResult = _queryResultFactory.CreateDefaultQueryResult();
            if (!CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                await _currentModbusMaster.WriteSingleRegisterAsync(_slaveId, registerAddress, valueToWrite);
                LogQuery(true, dataTitle, "Fun:6" + " Addr:" + registerAddress + " Data:" + valueToWrite);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                LogQuery(false, dataTitle, "Fun:6" + " Addr:" + registerAddress + " Data:" + valueToWrite,
                    exception: e);
                queryResult.IsSuccessful = false;
            }

            _semaphoreSlim.Release(1);
            return queryResult;
        }

        public bool LastQuerySucceed
        {
            get { return _lastQuerySucceed; }
        }

        protected virtual void LogQuery(bool isSuccessful, string dataTitle, string queryDescription,
            string queryResult = "", Exception exception = null)
        {
            _lastQuerySucceed = isSuccessful;
        }
    }
}