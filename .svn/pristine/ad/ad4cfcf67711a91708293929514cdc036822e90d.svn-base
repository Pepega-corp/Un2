using System.Collections.ObjectModel;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;

namespace Unicon2.Fragments.Journals.Editor.Interfaces
{
    public interface IUniconJournalEditorViewModel : IFragmentEditorViewModel, INameable
    {
        ObservableCollection<IJournalLoadingSequenceEditorViewModel> JournalLoadingSequenceEditorViewModels { get; set; }
        IJournalLoadingSequenceEditorViewModel SelectedJournalLoadingSequenceEditorViewModel { get; set; }
        IRecordTemplateEditorViewModel JournalRecordTemplateEditorViewModel { get; set; }
    }
}