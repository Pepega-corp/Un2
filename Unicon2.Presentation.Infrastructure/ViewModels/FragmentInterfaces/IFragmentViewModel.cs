using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentViewModel
    {
        string NameForUiKey { get; }
        IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        void Initialize(IDeviceFragment deviceFragment);
    }
}
