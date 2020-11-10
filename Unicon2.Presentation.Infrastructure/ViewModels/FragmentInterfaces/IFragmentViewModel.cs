using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentViewModel : IFragmentInitializable
    {
        string NameForUiKey { get; }
        IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        
    }

    public interface IFragmentConnectionChangedListener
    {
        void OnConnectionChanged();
    }
    public interface IFragmentOpenedListener
    {
        void OnFragmentOpened();
    }

    public interface IFragmentInitializable
    {
        void Initialize(IDeviceFragment deviceFragment);
    }
}