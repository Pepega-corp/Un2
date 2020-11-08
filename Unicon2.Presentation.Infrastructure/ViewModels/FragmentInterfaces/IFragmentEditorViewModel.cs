using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentEditorViewModel : IFragmentInitializable, IStronglyNamed
    {
        string NameForUiKey { get; }
        IDeviceFragment BuildDeviceFragment();
    }
}