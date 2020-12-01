using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Journals.Editor.Interfaces.JournalParameters;
using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Journals.Editor.Interfaces
{
    public interface IRecordTemplateEditorViewModel : IViewModel
    {
        ObservableCollection<IJournalParameterEditorViewModel> AllJournalParameterEditorViewModels { get; set; }

        IJournalParameterEditorViewModel SelectedJournalParameterEditorViewModel { get; set; }
        ICommand AddRecordParameterCommand { get; }
        ICommand AddComplexRecordParameterCommand { get; }
        ICommand AddDependentParameterCommand { get; }


        ICommand SetElementUpCommand { get; }
        ICommand SetElementDownCommand { get; }
        ICommand EditElementCommand { get; }
        ICommand DeleteElementCommand { get; }
        ICommand ShowFormatterParametersCommand { get; }
    }
}