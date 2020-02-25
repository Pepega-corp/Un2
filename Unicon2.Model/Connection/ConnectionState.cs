using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;
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
            ExpectedValues = new List<string>();
        }

        public bool IsConnected { get; private set; }

        public IDataProvider DataProvider
        {
            get { return (_deviceConnection as IDataProvider); }
        }

        public bool GetIsExpectedValueMatchesDevice()
        {
            bool isMatches = false;
            if (ExpectedValues == null) return false;
            if (TestResultValue == null) return false;
            foreach (var expectedValue in ExpectedValues)
            {
                string pattern = expectedValue.Replace(" ", "");
                if (Regex.IsMatch(TestResultValue.AsString().Replace(" ", ""), pattern, RegexOptions.IgnoreCase))
                {
                    isMatches = true;
                }

                if (expectedValue.Replace(" ", "") == TestResultValue.AsString().Replace(" ", "")) isMatches = true;
            }

            return isMatches;
        }

        public Action ConnectionStateChangedAction { get; set; }

        public async Task CheckConnection()
        {
            if (DeviceValueContaining is ILoadable)
            {
                if (_deviceConnection is IDataProvider)
                {
                    (DeviceValueContaining as ILoadable).DataProvider = _deviceConnection as IDataProvider;

                    await (DeviceValueContaining as ILoadable).Load();
                    OnConnectionTestValueChecked();
                }
                else
                {
                    IsConnected = false;
                    ConnectionStateChangedAction?.Invoke();
                }
            }
        }


        private void OnConnectionTestValueChecked()
        {
            IsConnected = (_deviceConnection as IDataProvider).LastQuerySucceed;
            if (!IsConnected)
            {
                TestResultValue = null;
                //_isConnected = _deviceConnection.TryOpenConnectionAsync(false, _deviceLogger);
            }
            else if (DeviceValueContaining is IUshortFormattable)
            {
                //TODO
               // TestResultValue =
               //     DeviceValueContaining.UshortsFormatter.Format(DeviceValueContaining.DeviceUshortsValue);
            }

            ConnectionStateChangedAction?.Invoke();
        }



        public IFormattedValue TestResultValue { get; set; }
        [DataMember] public IUshortFormattable DeviceValueContaining { get; set; }
        [DataMember] public List<string> ExpectedValues { get; set; }
        [DataMember] public IComPortConfiguration DefaultComPortConfiguration { get; set; }

        public void Initialize(IDeviceConnection deviceConnection, IDeviceLogger deviceLogger)
        {
            TestResultValue = null;
            _deviceLogger = deviceLogger;
            _deviceConnection = deviceConnection;
            _deviceConnection.LastQueryStatusChangedAction += (isLastQuerySucceed) =>
            {
                IsConnected = isLastQuerySucceed;
                CheckConnection();
            };
        }

        public async Task TryReconnect()
        {
            if (!IsConnected)
            {
                await _deviceConnection.TryOpenConnectionAsync(false, _deviceLogger);
            }
        }


        public object Clone()
        {
            IConnectionState connectionState = new DeviceConnectionState();
            if (ExpectedValues != null)
            {
                connectionState.ExpectedValues.AddRange(ExpectedValues);

            }

            connectionState.DeviceValueContaining = DeviceValueContaining?.Clone() as IUshortFormattable;
            connectionState.DefaultComPortConfiguration = DefaultComPortConfiguration?.Clone() as IComPortConfiguration;
            return connectionState;
        }
    }
}