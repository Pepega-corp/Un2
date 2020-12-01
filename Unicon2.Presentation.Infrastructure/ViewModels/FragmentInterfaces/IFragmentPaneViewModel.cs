using System;
using System.Windows.Input;
using Unicon2.Presentation.Infrastructure.ViewModels.DockingManagerWindows;

namespace Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces
{
    public interface IFragmentPaneViewModel : IDockingWindow
    {
        IFragmentViewModel FragmentViewModel { get; set; }
        ICommand CloseFragmentCommand { get; set; }
        Action<IFragmentPaneViewModel> FragmentPaneClosedAction { get; set; }
        string FragmentTitle { get; set; }
    }
}