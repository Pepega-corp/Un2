using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [DataContract(Namespace = "OscilloscopeJournalLoadingSequenceNS")]
    public class OscilloscopeJournalLoadingSequence : IOscilloscopeJournalLoadingSequence
    {
        private IDataProvider _dataProvider;
        private ushort _currentRecordNumber = 0;
        private ushort[] _currentRecordValues;
        private IOscilloscopeLoadingSequenceInitializingParameters _oscilloscopeLoadingSequenceInitializingParameters;

        public void SetDataProvider(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE_JOURNAL_LOADING_SEQUENCE;

        public void Initialize(IJournalSequenceInitializingParameters journalSequenceInitializingParameters)
        {
            _oscilloscopeLoadingSequenceInitializingParameters =
                journalSequenceInitializingParameters as IOscilloscopeLoadingSequenceInitializingParameters;
        }

        public async Task<bool> GetIsNextRecordAvailable()
        {
            IQueryResult writingRecordNumQueryResult = await _dataProvider.WriteSingleRegisterAsync(AddressOfRecord, _currentRecordNumber, "Write");
            _currentRecordNumber++;
            if (!writingRecordNumQueryResult.IsSuccessful) return false;
            IQueryResult<ushort[]> recordValuesQueryResult =
                await _dataProvider.ReadHoldingResgistersAsync(AddressOfRecord, NumberOfPointsInRecord, "Read");
            if (recordValuesQueryResult.IsSuccessful)
            {
                _currentRecordValues = recordValuesQueryResult.Result;

                ushort[] readyMarkUshorts = recordValuesQueryResult.Result
                    .Skip(_oscilloscopeLoadingSequenceInitializingParameters.AddressOfReadyMark)
                    .Take(_oscilloscopeLoadingSequenceInitializingParameters.NumberOfReadyMarkPoints).ToArray();
                int readyMark = readyMarkUshorts.GetIntFromTwoUshorts();
                if (_currentRecordValues.All((arg => arg == 0))) return false;
                if (readyMark == 0) return true;
            }
            return false;
        }

        public async Task<ushort[]> GetNextRecordUshorts()
        {
            return _currentRecordValues;
        }

        public void ResetSequence()
        {
            _currentRecordNumber = 0;
        }

        [DataMember]
        public ushort AddressOfRecord { get; set; }
        [DataMember]
        public ushort NumberOfPointsInRecord { get; set; }
    }
}
