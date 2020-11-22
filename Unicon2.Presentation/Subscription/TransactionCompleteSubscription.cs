using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
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
        private Action _onConnectionRetriesCounterOverflow;
        private IConnectionService _connectionService;
        private SemaphoreSlim _semaphoreSlim = new SemaphoreSlim(1);
        private ushort _lostConnectionRetriesCounter = 0;


        public TransactionCompleteSubscription(DeviceContext deviceContext, IConnectionState connectionState,
            IConnectionStateViewModel connectionStateViewModel, ITypesContainer container, Action onConnectionRetriesCounterOverflow)
        {
            _deviceContext = deviceContext;
            _connectionState = connectionState;
            _connectionStateViewModel = connectionStateViewModel;
            _container = container;
            _onConnectionRetriesCounterOverflow = onConnectionRetriesCounterOverflow;
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
            _isPrevCheckOffline = false;

            if (_deviceContext.DataProviderContainer.DataProvider.Item.LastQuerySucceed &&
                _connectionStateViewModel.TestValue != null)
            {
                _connectionStateViewModel.IsDeviceConnected = true;
                _isPreviousCheckSuccessful = true;
                _lostConnectionRetriesCounter = 0;
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
                        _lostConnectionRetriesCounter++;
                    }
                    else
                    {
                        _connectionStateViewModel.TestValue = res.Item;
                        _connectionStateViewModel.IsDeviceConnected = true;
                        _isPreviousCheckSuccessful = true;
                    }
                }
                else
                {
                    _lostConnectionRetriesCounter++;
                }
            }

            if (_lostConnectionRetriesCounter > 5)
            {
                _onConnectionRetriesCounterOverflow?.Invoke();
                _onConnectionRetriesCounterOverflow = null;
            }
        }

        private async Task TryDequeueAndExecute()
        {
            if (_semaphoreSlim.CurrentCount > 0)
            {
                try
                {
                    await _semaphoreSlim.WaitAsync();
                    await ExecuteAsync();
                }
                finally
                {
                    _semaphoreSlim.Release(1);
                }
            }
        }

        public async void Execute()
        {
            await TryDequeueAndExecute();
        }


        public int Priority { get; set; } = 1;
    }
}