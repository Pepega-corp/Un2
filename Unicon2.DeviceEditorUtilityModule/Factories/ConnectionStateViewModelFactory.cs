using Unicon2.DeviceEditorUtilityModule.Interfaces;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Connection;

namespace Unicon2.DeviceEditorUtilityModule.Factories
{
    public class ConnectionStateViewModelFactory : IConnectionStateViewModelFactory
    {
        public IConnectionStateViewModel CreateConnectionStateViewModel(IConnectionState connectionState)
        {
            IConnectionStateViewModel connectionStateViewModel = StaticContainer.Container.Resolve<IConnectionStateViewModel>();
            connectionStateViewModel.Model = connectionState;
            return connectionStateViewModel;
        }
    }
}
