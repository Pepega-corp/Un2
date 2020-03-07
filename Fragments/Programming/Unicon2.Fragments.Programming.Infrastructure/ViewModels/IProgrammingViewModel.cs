using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels
{
    public interface IProgrammingViewModel: IFragmentViewModel
    {
        ObservableCollection<ISchemeTabViewModel> SchemesCollection { get; }
        ObservableCollection<IConnectionViewModel> ConnectionCollection { get; }
        void AddConnection(IConnectionViewModel connectionVoewModel);
        void RemoveConnection(IConnectionViewModel connectionVoewModel);

        int SelectedTabIndex { get; set; }

        ICommand NewSchemeCommand { get; }
        ICommand CloseTabCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand ZoomIncrementCommand { get; }
        ICommand ZoomDecrementCommand { get; }
    }
}