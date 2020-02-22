using Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Properties;
using Unicon2.Presentation.Infrastructure.ViewModels;

namespace Unicon2.Fragments.Configuration.Infrastructure.ViewModel.Runtime
{
    public interface IRuntimePropertyViewModel : IPropertyViewModel, IRuntimeConfigurationItemViewModel,
        ILocalAndDeviceValueContainingViewModel
    {
    }
}