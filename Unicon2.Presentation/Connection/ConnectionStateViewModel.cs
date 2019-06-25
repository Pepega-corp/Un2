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
            this._valueViewModelFactory = valueViewModelFactory;
            this.CheckConnectionCommand = new RelayCommand(OnCheckConnectionExecute);

            _semaphoreSlim = new SemaphoreSlim(1);
            IndicatorOpacity = 1;


        }

        private void OnCheckConnectionExecute()
        {
            this._connectionState.TryReconnect();
            this._connectionState.CheckConnection();
        }


        #region Implementation of IStronglyNamed

        public string StrongName { get; }

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this._connectionState; }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            if (value is IConnectionState)
            {
                IConnectionState connectionState = (value as IConnectionState);
                this._connectionState = connectionState;


                this._connectionState.ConnectionStateChangedAction += () =>
                {
                    if (_connectionState.DataProvider is IDataProvider)
                        _connectionState.DataProvider.TransactionCompleteAction += () => { this?.BeginIndication(); };
                    this.IsDeviceConnected = this._connectionState.IsConnected;
                    if (this.IsDeviceConnected)
                    {
                        this.TestValueViewModel =
                            this._valueViewModelFactory.CreateFormattedValueViewModel(this._connectionState.TestResultValue);
                    }
                    this.RaisePropertyChanged(nameof(this.IsDeviceConnected));
                };
                this._connectionState.ConnectionStateChangedAction?.Invoke();
            }
        }

        #endregion

        #region Implementation of IConnectionStateViewModel

        public bool IsDeviceConnected { get; private set; }

        public double IndicatorOpacity
        {
            get { return this._indicatorOpacity; }
            set
            {
                this._indicatorOpacity = value;
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

        #endregion
    }
}
