using System.Collections.Generic;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;

namespace Unicon2.Fragments.Journals.Infrastructure.Factories
{
    public interface IJournalSequenceEditorViewModelFactory
    {
        IJournalLoadingSequenceEditorViewModel CreateJournalLoadingSequenceEditorViewModel(
            IJournalLoadingSequence journalLoadingSequence);

        List<IJournalLoadingSequenceEditorViewModel> GetAvailableLoadingSequenceEditorViewModels();

    }
}