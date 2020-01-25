using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels
{
    public interface IInputEditorViewModel : ILogicElementEditorViewModel
    {
        ICommand AddBaseCommand { get; }
        ICommand RemoveBaseCommand { get; }
        ICommand AddSignalCommand { get; }
        ICommand RemoveSignalCommand { get; }
        ObservableCollection<EditableListItem> Bases { get; }
        ObservableCollection<BindableKeyValuePair<int, string>> InputSignals { get; }
        BindableKeyValuePair<int, string> SelectedInputSignal { get; set; }
    }
}