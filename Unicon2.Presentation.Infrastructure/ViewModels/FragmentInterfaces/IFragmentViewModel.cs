using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentViewModel : IFragmentInitializable
    {
        string NameForUiKey { get; }
        IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

    }

    public interface IFragmentInitializable
    {
        void Initialize(IDeviceFragment deviceFragment);
    }
}