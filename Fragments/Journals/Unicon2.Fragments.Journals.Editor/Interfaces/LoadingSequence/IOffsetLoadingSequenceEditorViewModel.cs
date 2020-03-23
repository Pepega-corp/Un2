using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.LoadingSequence
{
    public interface IOffsetLoadingSequenceEditorViewModel : IJournalLoadingSequenceEditorViewModel
    {
        int NumberOfRecords { get; set; }
        ushort JournalStartAddress { get; set; }
        ushort NumberOfPointsInRecord { get; set; }
        ushort WordFormatFrom { get; set; }
        ushort WordFormatTo { get; set; }
        bool IsWordFormatNotForTheWholeRecord { get; set; }
    }
}