using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimePropertyViewModel : IPropertyViewModel, IRuntimeConfigurationItemViewModel,
        ILocalAndDeviceValueContainingViewModel, IDeviceContextConsumer,IBitsConfigViewModel,IFormattedValueOwner
    {
        ushort Address { get; set; }
        ushort NumberOfPoints { get; set; }
        bool IsHidden { get; set; }
    }
}