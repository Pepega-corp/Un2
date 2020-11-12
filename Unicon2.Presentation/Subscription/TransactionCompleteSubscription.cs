using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.Connection;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Presentation.Subscription
{
    public class TransactionCompleteSubscription : IDeviceSubscription
    {
        private readonly DeviceContext _deviceContext;
        private readonly IConnectionState _connectionState;
        private readonly IConnectionStateViewModel _connectionStateViewModel;
        private readonly ITypesContainer _container;
        private IConnectionService _connectionService;
        private Result<Task> _currentTask = Result<Task>.Create(false);

        public TransactionCompleteSubscription(DeviceContext deviceContext, IConnectionState connectionState,
            IConnectionStateViewModel connectionStateViewModel, ITypesContainer container)
        {
            _deviceContext = deviceContext;
            _connectionState = connectionState;
            _connectionStateViewModel = connectionStateViewModel;
            _container = container;
            _connectionService = container.Resolve<IConnectionService>();
        }

        private bool _isPreviousCheckSuccessful = true;
        private bool _isPrevCheckOffline = false;


        public async Task ExecuteAsync()
        {
            if (!_deviceContext.DataProviderContainer.DataProvider.IsSuccess)
            {
                _connectionStateViewModel.IsDeviceConnected = false;
                _isPrevCheckOffline = true;
                return;
            }

            if (_deviceContext.DataProviderContainer.DataProvider.Item.LastQuerySucceed &&
                _connectionStateViewModel.TestValue != null)
            {
                _connectionStateViewModel.IsDeviceConnected = true;
                _isPreviousCheckSuccessful = true;
            }
            else
            {
                if (_isPreviousCheckSuccessful || _isPrevCheckOffline)
                {
                    var res = await _connectionService.CheckConnection(_connectionState, _deviceContext);
                    if (!res.IsSuccess)
                    {
                        _connectionStateViewModel.IsDeviceConnected = false;
                        _isPreviousCheckSuccessful = false;
                    }
                    else
                    {
                        _connectionStateViewModel.TestValue = res.Item;
                        _connectionStateViewModel.IsDeviceConnected = true;
                        _isPreviousCheckSuccessful = true;
                    }
                }

            }
        }

        public async void Execute()
        {
            if (_currentTask.IsSuccess && !_currentTask.Item.IsCompleted)
            {
                _currentTask.Item.Dispose();
            }
            _currentTask = Result<Task>.Create(ExecuteAsync(), true);
            await _currentTask.Item;
        }

        public int Priority { get; set; } = 1;
    }
}