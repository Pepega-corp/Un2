using System.Windows.Input;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.ViewModels.Fragment.FragmentOptions
{
    public class DefaultFragmentOptionCommandViewModel : IFragmentOptionCommandViewModel
    {
        public string TitleKey { get; set; }

        public ICommand OptionCommand { get; set; }

        public string Title => StaticContainer.Container.Resolve<ILocalizerService>().GetLocalizedString(TitleKey);

        public string IconKey { get; set; }
    }
}