using Unicon2.Infrastructure.ViewModel;

namespace Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence
{
    public interface IJournalLoadingSequenceEditorViewModel : IViewModel
    {
        string NameForUiKey { get; }
    }
}