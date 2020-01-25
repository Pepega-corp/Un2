using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels
{
    public interface IOutputEditorViewModel : ILogicElementEditorViewModel
    {
        ObservableCollection<EditableListItem> OutputSignals { get; }
        ICommand AddOutputSignalCommand { get; }
        ICommand RemoveOutputSignalCommand { get; }
    }
}