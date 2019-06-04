using NModbus4.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.DataProvider.Model
{
    [DataContract]
    public abstract class ModbusDataProvider : Disposable, IDataProvider, IInitializableFromContainer
    {
        private IQueryResultFactory _queryResultFactory;
        protected bool _isInitialized;
        protected IModbusMaster _currentModbusMaster;
        protected byte _slaveId = 0; //for debug number of device =1. Defaul 0
        protected bool _lastQuerySucceed;

        public Action TransactionCompleteAction { get; set; }

        protected virtual bool CheckConnection(IQueryResult queryResult)
        {
            if (this._currentModbusMaster.Transport == null)
            {
                queryResult.IsSuccessful = false;
                return false;
            }
            return true;
        }

        protected ModbusDataProvider(IQueryResultFactory queryResultFactory)
        {
            this._queryResultFactory = queryResultFactory;
        }


        #region Implementation of IDataProvider

        public async Task<IQueryResult<ushort[]>> ReadHoldingResgistersAsync(ushort startAddress, ushort numberOfPoints, string dataTitle)
        {
            IQueryResult<ushort[]> queryResult = this._queryResultFactory.CreateDefaultQueryResult<ushort[]>();
            if (!this.CheckConnection(queryResult)) return queryResult;
            try
            {
                TransactionCompleteAction?.Invoke();
                queryResult.Result = await this._currentModbusMaster.ReadHoldingRegistersAsync(this._slaveId, startAddress, numberOfPoints);
                List<string> results = queryResult.Result.Select((arg => arg.ToString())).ToList();
                string resStr = "";
                foreach (string res in results)
                {
                    resStr += res;
                    resStr += " ";
                }
                this.LogQuery(true, dataTitle, "Fun:3" + " Addr:" + startAddress + " Num:" + numberOfPoints + " Data:" + resStr);
                queryResult.IsSuccessful = true;

            }
            catch (Exception e)
            {
                this.LogQuery(false, dataTitle, "Fun:3" + " Addr:" + startAddress + " Num:" + numberOfPoints, exception: e);
                queryResult.IsSuccessful = false;
            }
            return queryResult;
        }

        public async Task<IQueryResult<bool>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle)
        {
            IQueryResult<bool> queryResult = this._queryResultFactory.CreateDefaultQueryResult<bool>();
            if (!this.CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                queryResult.Result = (await this._currentModbusMaster.ReadCoilsAsync(this._slaveId, coilAddress, 1))[0];
                this.LogQuery(true, dataTitle, "Fun:1" + " Addr:" + coilAddress + " Num:" + 1 + " Data:" + queryResult.Result);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                this.LogQuery(false, dataTitle, "Fun:1" + " Addr:" + coilAddress + " Num:" + 1, exception: e);
                queryResult.IsSuccessful = false;
            }
            return queryResult;
        }

        public async Task<IQueryResult<bool[]>> ReadCoilStatusAsync(ushort coilAddress, string dataTitle, ushort numberOfPoints)
        {
            IQueryResult<bool[]> queryResult = this._queryResultFactory.CreateDefaultQueryResult<bool[]>();
            if (!this.CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                queryResult.Result = await this._currentModbusMaster.ReadCoilsAsync(this._slaveId, coilAddress, numberOfPoints);
                string resStr = "";
                foreach (bool res in queryResult.Result)
                {
                    resStr += res;
                    resStr += " ";
                }
                this.LogQuery(true, dataTitle, "Fun:1" + " Addr:" + coilAddress + " Num:" + numberOfPoints + " Data:" + resStr);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                this.LogQuery(false, dataTitle, "Fun:1" + " Addr:" + coilAddress + " Num:" + numberOfPoints, exception: e);
                queryResult.IsSuccessful = false;
            }
            return queryResult;
        }

        public async Task<IQueryResult> WriteMultipleRegistersAsync(ushort startAddress, ushort[] dataToWrite, string dataTitle)
        {
            IQueryResult queryResult = this._queryResultFactory.CreateDefaultQueryResult();
            if (!this.CheckConnection(queryResult)) return queryResult;

            string dataStr = "";
            foreach (ushort res in dataToWrite)
            {
                dataStr += res;
                dataStr += " ";
            }
            try
            {
                TransactionCompleteAction?.Invoke();
                await this._currentModbusMaster.WriteMultipleRegistersAsync(this._slaveId, startAddress, dataToWrite);

                this.LogQuery(true, dataTitle, "Fun:16" + " Addr:" + startAddress + " Data:" + dataStr);

                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                this.LogQuery(false, dataTitle, "Fun:16" + " Addr:" + startAddress + " Data:" + dataStr, exception: e);
                queryResult.IsSuccessful = false;

            }
            return queryResult;
        }



        public async Task<IQueryResult> WriteSingleCoilAsync(ushort coilAddress, bool valueToWrite, string dataTitle)
        {
            IQueryResult queryResult = this._queryResultFactory.CreateDefaultQueryResult();
            if (!this.CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                await this._currentModbusMaster.WriteSingleCoilAsync(this._slaveId, coilAddress, valueToWrite);
                this.LogQuery(true, dataTitle, "Fun:5" + " Addr:" + coilAddress + " Data:" + valueToWrite);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                this.LogQuery(false, dataTitle, "Fun:5" + " Addr:" + coilAddress + " Data:" + valueToWrite, exception: e);
                queryResult.IsSuccessful = false;
            }
            return queryResult;
        }

        public async Task<IQueryResult> WriteSingleRegisterAsync(ushort registerAddress, ushort valueToWrite, string dataTitle)
        {
            IQueryResult queryResult = this._queryResultFactory.CreateDefaultQueryResult();
            if (!this.CheckConnection(queryResult)) return queryResult;

            try
            {
                TransactionCompleteAction?.Invoke();
                await this._currentModbusMaster.WriteSingleRegisterAsync(this._slaveId, registerAddress, valueToWrite);
                this.LogQuery(true, dataTitle, "Fun:6" + " Addr:" + registerAddress + " Data:" + valueToWrite);
                queryResult.IsSuccessful = true;
            }
            catch (Exception e)
            {
                this.LogQuery(false, dataTitle, "Fun:6" + " Addr:" + registerAddress + " Data:" + valueToWrite, exception: e);
                queryResult.IsSuccessful = false;
            }
            return queryResult;
        }

        public bool LastQuerySucceed
        {
            get
            {
                return this._lastQuerySucceed;

            }
        }


        protected virtual void LogQuery(bool isSuccessful, string dataTitle, string queryDescription,
            string queryResult = "", Exception exception = null)
        {
            this._lastQuerySucceed = isSuccessful;
        }


        #endregion

        #region Implementation of IInitializableFromContainer

        public bool IsInitialized
        {
            get { return this._isInitialized; }
        }

        public virtual void InitializeFromContainer(ITypesContainer container)
        {
            this._queryResultFactory = container.Resolve<IQueryResultFactory>();
            this._isInitialized = true;
        }

        #endregion
    }
}
