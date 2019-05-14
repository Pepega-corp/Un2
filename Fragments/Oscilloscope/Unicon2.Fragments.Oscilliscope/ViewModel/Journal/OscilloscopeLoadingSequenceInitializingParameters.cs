using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;

namespace Unicon2.Fragments.Oscilliscope.ViewModel.Journal
{
   public class OscilloscopeLoadingSequenceInitializingParameters: IOscilloscopeLoadingSequenceInitializingParameters
    {
        public ushort AddressOfReadyMark { get; set; }
        public ushort NumberOfReadyMarkPoints { get; set; }
    }
}
