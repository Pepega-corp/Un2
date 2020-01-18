using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
using Unicon2.Infrastructure.Interfaces.Values;
using Unicon2.Infrastructure.Services.LogService;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Model.Connection
{
    [DataContract(Namespace = "DeviceConnectionStateNS")]
    public class DeviceConnectionState : IConnectionState
    {
        private IDeviceConnection _deviceConnection;
        private IDeviceLogger _deviceLogger;
        
        public DeviceConnectionState()
        {
            this.ExpectedValues = new List<string>();
        }

        public bool IsConnected { get; private set; }
        public IDataProvider DataProvider { get { return (_deviceConnection as IDataProvider); } }

        public bool GetIsExpectedValueMatchesDevice()
        {
            bool isMatches = false;
            if (this.ExpectedValues == null) return false;
            if (this.TestResultValue == null) return false;
            foreach (var expectedValue in this.ExpectedValues)
            {
                string pattern = expectedValue.Replace(" ", "");
                if (Regex.IsMatch(this.TestResultValue.AsString().Replace(" ", ""), pattern, RegexOptions.IgnoreCase))
                {
                    isMatches = true;
                }
                if (expectedValue.Replace(" ", "") == this.TestResultValue.AsString().Replace(" ", "")) isMatches = true;
            }

            return isMatches;
        }

        public Action ConnectionStateChangedAction { get; set; }

        public async Task CheckConnection()
        {
            if (this.DeviceValueContaining is ILoadable)
            {
                if (this._deviceConnection is IDataProvider)
                {
                    (this.DeviceValueContaining as ILoadable).SetDataProvider(this._deviceConnection as IDataProvider);

                    await (this.DeviceValueContaining as ILoadable).Load();
                    this.OnConnectionTestValueChecked();
                }
                else
                {
                    this.IsConnected = false;
                    this.ConnectionStateChangedAction?.Invoke();
                }
            }
        }


        private void OnConnectionTestValueChecked()
        {
            this.IsConnected = (this._deviceConnection as IDataProvider).LastQuerySucceed;
            if (!this.IsConnected)
            {
                this.TestResultValue = null;
                //_isConnected = _deviceConnection.TryOpenConnectionAsync(false, _deviceLogger);
            }
            else if (this.DeviceValueContaining is IUshortFormattable)
            {
                this.TestResultValue = (this.DeviceValueContaining as IUshortFormattable).UshortsFormatter.Format(this.DeviceValueContaining.DeviceUshortsValue);
            }
            this.ConnectionStateChangedAction?.Invoke();
        }



        public IFormattedValue TestResultValue { get; set; }
        [DataMember]
        public IDeviceValueContaining DeviceValueContaining { get; set; }
        [DataMember]
        public List<string> ExpectedValues { get; set; }
        [DataMember]
        public IComPortConfiguration DefaultComPortConfiguration { get; set; }

        public void Initialize(IDeviceConnection deviceConnection, IDeviceLogger deviceLogger)
        {
            this.TestResultValue = null;
            ((this.DeviceValueContaining as IUshortFormattable)?.UshortsFormatter as IInitializableFromContainer)?.InitializeFromContainer(StaticContainer.Container);
            this._deviceLogger = deviceLogger;
            this._deviceConnection = deviceConnection;
            this._deviceConnection.LastQueryStatusChangedAction += (isLastQuerySucceed) =>
             {
                 this.IsConnected = isLastQuerySucceed;
                 this.CheckConnection();
             };
        }

        public async Task TryReconnect()
        {
            if (!this.IsConnected)
            {
               await this._deviceConnection.TryOpenConnectionAsync(false, this._deviceLogger);
            }
        }


        public object Clone()
        {
            IConnectionState connectionState=new DeviceConnectionState();
            if (this.ExpectedValues != null)
            {
                connectionState.ExpectedValues.AddRange(this.ExpectedValues);

            }
            connectionState.DeviceValueContaining = this.DeviceValueContaining?.Clone() as IDeviceValueContaining;
            connectionState.DefaultComPortConfiguration = this.DefaultComPortConfiguration?.Clone() as IComPortConfiguration;
            return connectionState;
        }
    }
}
