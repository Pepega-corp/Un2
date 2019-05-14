using System;
using System.Windows.Input;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ISchemeTabViewModel : IViewModel
    {
        event Action CloseTabEvent;

        string SchemeName { get; set; }
        double SchemeHeight { get; set; }
        double SchemeWidth { get; set; }
        double Scale { get; set; }

        ICommand ZoomIncrementCommand { get; }
        ICommand ZoomDecrementCommand { get; }
        ICommand DeleteCommand { get; }
    }
}