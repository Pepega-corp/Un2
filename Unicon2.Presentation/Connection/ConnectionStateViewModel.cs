using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.ViewModels.Connection;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.Connection
{
    public class ConnectionStateViewModel : ViewModelBase, IConnectionStateViewModel
    {
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private IConnectionState _connectionState;
        private double _indicatorOpacity;
        private SemaphoreSlim _semaphoreSlim;


        public ConnectionStateViewModel(IValueViewModelFactory valueViewModelFactory)
        {
            _valueViewModelFactory = valueViewModelFactory;
            CheckConnectionCommand = new RelayCommand(OnCheckConnectionExecute);

            _semaphoreSlim = new SemaphoreSlim(1);
            IndicatorOpacity = 1;


        }

        private void OnCheckConnectionExecute()
        {
            _connectionState.TryReconnect();
            _connectionState.CheckConnection();
        }


        public string StrongName { get; }

        public object Model
        {
            get { return _connectionState; }
            set { SetModel(value); }
        }

        private void SetModel(object value)
        {
            if (value is IConnectionState)
            {
                IConnectionState connectionState = (value as IConnectionState);
                _connectionState = connectionState;


                _connectionState.ConnectionStateChangedAction += () =>
                {
                    if (_connectionState.DataProvider is IDataProvider)
                        _connectionState.DataProvider.TransactionCompleteAction += () => { this?.BeginIndication(); };
                    IsDeviceConnected = _connectionState.IsConnected;
                    if (IsDeviceConnected)
                    {
                        TestValueViewModel =
                            _valueViewModelFactory.CreateFormattedValueViewModel(_connectionState.TestResultValue);
                    }
                    RaisePropertyChanged(nameof(IsDeviceConnected));
                };
                _connectionState.ConnectionStateChangedAction?.Invoke();
            }
        }

        public bool IsDeviceConnected { get; private set; }

        public double IndicatorOpacity
        {
            get { return _indicatorOpacity; }
            set
            {
                _indicatorOpacity = value;
                RaisePropertyChanged();
            }
        }
        private async Task BeginIndication()
        {
            try
            {
                if (_semaphoreSlim.CurrentCount > 0)
                {
                    await _semaphoreSlim.WaitAsync();

                    IndicatorOpacity = 0.2;
                    await Task.Delay(300);
                    IndicatorOpacity = 1;
                    _semaphoreSlim.Release(1);
                }
            }
            finally
            {
                _semaphoreSlim.Release(1);
            }
        }

        public IFormattedValueViewModel TestValueViewModel { get; set; }
        public ICommand CheckConnectionCommand { get; }
    }
}
