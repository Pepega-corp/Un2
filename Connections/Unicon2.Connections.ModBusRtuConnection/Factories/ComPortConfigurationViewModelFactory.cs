using Unicon2.Connections.ModBusRtuConnection.Interfaces.Factories;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Device;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Connections.ModBusRtuConnection.Factories
{
    public class ComPortConfigurationViewModelFactory : IComPortConfigurationViewModelFactory
    {
        private readonly ITypesContainer _container;

        public ComPortConfigurationViewModelFactory(ITypesContainer container)
        {
            _container = container;
        }

        public IComPortConfigurationViewModel CreateComPortConfigurationViewModel(IComPortConfiguration comPortConfiguration)
        {
            IComPortConfigurationViewModel comPortConfigurationViewModel = _container.Resolve<IComPortConfigurationViewModel>();
            comPortConfigurationViewModel.Model = comPortConfiguration;
            return comPortConfigurationViewModel;
        }
    }
}
