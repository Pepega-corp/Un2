
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Infrastructure.Connection;
using Unicon2.Presentation.Connection;
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

        public TransactionCompleteSubscription(DeviceContext deviceContext, IConnectionState connectionState,
            IConnectionStateViewModel connectionStateViewModel, ITypesContainer container)
        {
            this._deviceContext = deviceContext;
            this._connectionState = connectionState;
            this._connectionStateViewModel = connectionStateViewModel;
            this._container = container;
            _connectionService = container.Resolve<IConnectionService>();
        }

        private bool _isPreviousCheckSuccessful = true;

        public async void Execute()
        {
            if (this._deviceContext.DataProviderContainer.DataProvider.LastQuerySucceed&& this._connectionStateViewModel.TestValue!=null)
            {
                this._connectionStateViewModel.IsDeviceConnected = true;
                _isPreviousCheckSuccessful = true;
            }
            else
            {
	            if (_isPreviousCheckSuccessful)
	            {
		            var res = await this._connectionService.CheckConnection(this._connectionState, this._deviceContext);
		            if (!res.IsSuccess)
		            {
			            this._connectionStateViewModel.IsDeviceConnected = false;
			            _isPreviousCheckSuccessful = false;
		            }
		            else
		            {
			            this._connectionStateViewModel.TestValue = res.Item;
			            this._connectionStateViewModel.IsDeviceConnected = true;
			            _isPreviousCheckSuccessful = true;
		            }
	            }

            }
        }
    }
}