using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;

namespace Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence
{
    public interface IOscilloscopeJournalLoadingSequence:IJournalLoadingSequence
    {
        ushort AddressOfRecord { get; set; }
        ushort NumberOfPointsInRecord { get; set; }

    }

    public interface IJournalSequenceInitialingFromParameters
    {
        void Initialize(IJournalSequenceInitializingParameters parameters);
    }
}