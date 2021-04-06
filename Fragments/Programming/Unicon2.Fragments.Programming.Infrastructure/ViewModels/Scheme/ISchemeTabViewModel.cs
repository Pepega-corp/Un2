using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme
{
    public interface ISchemeTabViewModel : IViewModel<ISchemeModel>
    {
        event Action<ISchemeTabViewModel> CloseTabEvent;
        bool CanWriteToDevice { get; }
        string SchemeName { get; set; }
        double SchemeHeight { get;  }
        double SchemeWidth { get; }
        double Scale { get; set; }
        string ScaleStr { get; }
        bool IsLogicStarted { get; set; }
        ObservableCollection<ISchemeElementViewModel> ElementCollection { get; }

        ICommand ZoomIncrementCommand { get; }
        ICommand ZoomDecrementCommand { get; }
        ICommand DeleteCommand { get; }
    }
}