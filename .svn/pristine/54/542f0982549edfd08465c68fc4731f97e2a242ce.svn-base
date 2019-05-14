using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Interfaces.EditOperations;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters
{
    public interface IDependentJournalParameterEditorViewModel : IJournalParameterEditorViewModel,IEditable
    {
        ICommand AddConditionCommand { get; }
        ICommand DeleteConditionCommand { get; }
        ICommand ShowFormatterParameters { get; }
        ICommand SubmitCommand { get; }
        ICommand CancelCommand { get; }

        ObservableCollection<IJournalConditionEditorViewModel> JournalConditionEditorViewModels { get; }
     void   SetAvaliableJournalParameters( List<IJournalParameter> availableJournalParameters);
    }
}