using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Presentation.Infrastructure.Factories
{
    public interface IFragmentEditorViewModelFactory
    {
        IFragmentEditorViewModel CreateFragmentEditorViewModel(IDeviceFragment deviceFragment);
    }
}