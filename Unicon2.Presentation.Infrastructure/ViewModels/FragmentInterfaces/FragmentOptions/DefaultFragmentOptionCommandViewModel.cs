using System.Windows.Input;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions
{
    public class DefaultFragmentOptionCommandViewModel :ViewModelBase, IFragmentOptionCommandViewModel
    {
        public string TitleKey { get; set; }

        public ICommand OptionCommand { get; set; }

        public virtual string Title => StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(TitleKey);

        public string IconKey { get; set; }
        public virtual void UpdateAvailability()
        {
            (OptionCommand as RelayCommand)?.RaiseCanExecuteChanged();
            (OptionCommand as RelayCommand<bool?>)?.RaiseCanExecuteChanged();
        }
    }
}