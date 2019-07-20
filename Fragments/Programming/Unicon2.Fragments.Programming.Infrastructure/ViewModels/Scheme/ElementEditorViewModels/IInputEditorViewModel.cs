using System.Collections.ObjectModel;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels
{
    public interface IInputEditorViewModel : ILogicElementEditorViewModel
    {
        ObservableCollection<string> Bases { get; }
        ObservableCollection<BindableKeyValuePair<int, string>> InputSignals { get; }
        BindableKeyValuePair<int, string> SelectedInputSignal { get; set; }
    }
}