using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters
{
    public interface IComplexJournalParameterEditorViewModel : IJournalParameterEditorViewModel
    {
        ISubJournalParameterEditorViewModel AddSubJournalParameterEditorViewModel();
        ICommand AddSubParameterCommand { get; }
        ICommand DeleteParameterCommand { get; }
        ISubJournalParameterEditorViewModel SelectedSubJournalParameterEditorViewModel { get; set; }

        ObservableCollection<ISubJournalParameterEditorViewModel> SubJournalParameterEditorViewModels { get; set; }
        ObservableCollection<ISharedBitViewModel> MainBitNumbersInWordCollection { get; set; }
        ICommand SubmitCommand { get; }
        ICommand CancelCommand { get; }


    }

}