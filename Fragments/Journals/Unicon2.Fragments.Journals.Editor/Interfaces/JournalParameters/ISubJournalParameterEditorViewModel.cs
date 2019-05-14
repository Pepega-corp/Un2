using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters
{
    public interface ISubJournalParameterEditorViewModel:IJournalParameterEditorViewModel
    {
        ObservableCollection<ISharedBitViewModel> BitNumbersInWord { get; set; }
        ICommand ShowFormatterParametersCommand { get; }

    }
}