using Unicon2.Fragments.Journals.Infrastructure.ViewModel.LoadingSequence;

namespace Unicon2.Fragments.Journals.Editor.Interfaces.LoadingSequence
{
    public interface IIndexLoadingSequenceEditorViewModel : IJournalLoadingSequenceEditorViewModel
    {
        ushort JournalStartAddress { get; set; }
        ushort NumberOfPointsInRecord { get; set; }
        ushort WordFormatFrom { get; set; }
        ushort WordFormatTo { get; set; }
        bool IsWordFormatNotForTheWholeRecord { get; set; }
        ushort IndexWritingAddress { get; set; }
        bool WriteIndexOnlyFirstTime { get; set; }

    }
}