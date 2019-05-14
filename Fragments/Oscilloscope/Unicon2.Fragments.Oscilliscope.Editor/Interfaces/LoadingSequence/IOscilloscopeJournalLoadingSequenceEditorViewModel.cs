using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;

namespace Unicon2.Fragments.Oscilliscope.Editor.Interfaces.LoadingSequence
{
    public interface IOscilloscopeJournalLoadingSequenceEditorViewModel: IJournalLoadingSequenceEditorViewModel
    {
        ushort AddressOfRecord { get; set; }
        ushort NumberOfPointsInRecord { get; set; }
    }
}