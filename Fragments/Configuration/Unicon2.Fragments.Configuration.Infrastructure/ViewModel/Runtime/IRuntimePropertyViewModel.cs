using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimePropertyViewModel : IPropertyViewModel, IRuntimeConfigurationItemViewModel,
        ILocalAndDeviceValueContainingViewModel, IDeviceContextConsumer
    {
        ushort Address { get; set; }
    }
}