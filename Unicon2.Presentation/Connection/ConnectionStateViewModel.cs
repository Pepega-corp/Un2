using System.Windows.Input;
using System.Threading.Tasks;
using System.Threading;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Connection;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.Connection
{
    public class ConnectionStateViewModel : ViewModelBase, IConnectionStateViewModel
    {
        private readonly IValueViewModelFactory _valueViewModelFactory;
        private readonly IConnectionService _connectionService;
        private double _indicatorOpacity;
        private SemaphoreSlim _semaphoreSlim;
        private bool _isDeviceConnected;
        private string _testValueViewModel;


        public ConnectionStateViewModel(IValueViewModelFactory valueViewModelFactory,
            IConnectionService connectionService)
        {
            _valueViewModelFactory = valueViewModelFactory;
            this._connectionService = connectionService;
            CheckConnectionCommand = new RelayCommand(OnCheckConnectionExecute);

            _semaphoreSlim = new SemaphoreSlim(1);
            IndicatorOpacity = 1;


        }

        private void OnCheckConnectionExecute()
        {

        }

        public bool IsDeviceConnected
        {
            get { return this._isDeviceConnected; }
            set
            {
                if (this._isDeviceConnected == value)
                {
                    return;
                }

                this._isDeviceConnected = value;
                RaisePropertyChanged();
            }
        }

        public double IndicatorOpacity
        {
            get { return _indicatorOpacity; }
            set
            {
                _indicatorOpacity = value;
                RaisePropertyChanged();
            }
        }

        public async Task BeginIndication()
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

        public string TestValue
        {
            get { return this._testValueViewModel; }
            set
            {
                this._testValueViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CheckConnectionCommand { get; }
    }
}