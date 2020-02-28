using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentEditorViewModel : IViewModel
    {
        string NameForUiKey { get; }
    }
}