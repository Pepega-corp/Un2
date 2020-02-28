using System.Collections.ObjectModel;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.FragmentSettings;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentSettings
{
    public interface IFragmentSettingsViewModel : IViewModel
    {
        ObservableCollection<IFragmentSettingViewModel> ConfigurationSettingViewModelCollection { get; }

    }
}