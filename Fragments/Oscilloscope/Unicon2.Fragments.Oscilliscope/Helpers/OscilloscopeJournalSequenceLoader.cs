using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model.Loader;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscilloscopeJournalLoadingSequence;
using Unicon2.Fragments.Oscilliscope.Model;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Functional;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Helpers
{
    public class OscilloscopeJournalSequenceLoader : ISequenceLoader, IJournalSequenceInitialingFromParameters
    {
        private OscilloscopeJournalLoadingSequence _oscilloscopeJournalLoadingSequence;
        private IDataProviderContainer _dataProviderContainer;
        private ushort _currentRecordNumber = 0;
        private IOscilloscopeLoadingSequenceInitializingParameters _oscilloscopeLoadingSequenceInitializingParameters;
        public OscilloscopeJournalSequenceLoader(OscilloscopeJournalLoadingSequence oscilloscopeJournalLoadingSequence, IDataProviderContainer dataProviderContainer)
        {
            _oscilloscopeJournalLoadingSequence = oscilloscopeJournalLoadingSequence;
            _dataProviderContainer = dataProviderContainer;
        }

        public bool GetIsNextRecordAvailable()
        {
            return true;
        }

        public async Task<Result<ushort[]>> GetNextRecordUshorts()
        {
            var res = await _dataProviderContainer.DataProvider.OnSuccessAsync<ushort[]>(async provider =>
            {
                IQueryResult writingRecordNumQueryResult =
                    await provider.WriteSingleRegisterAsync(_oscilloscopeJournalLoadingSequence.AddressOfRecord,
                        _currentRecordNumber, "Write");
                _currentRecordNumber++;
                if (!writingRecordNumQueryResult.IsSuccessful) return Result<ushort[]>.Create(false);
                IQueryResult<ushort[]> recordValuesQueryResult =
                    await provider.ReadHoldingResgistersAsync(_oscilloscopeJournalLoadingSequence.AddressOfRecord,
                        _oscilloscopeJournalLoadingSequence.NumberOfPointsInRecord, "Read");
                if (recordValuesQueryResult.IsSuccessful)
                {
                    ushort[] readyMarkUshorts = recordValuesQueryResult.Result
                        .Skip(_oscilloscopeLoadingSequenceInitializingParameters.AddressOfReadyMark)
                        .Take(_oscilloscopeLoadingSequenceInitializingParameters.NumberOfReadyMarkPoints).ToArray();
                    int readyMark = readyMarkUshorts.GetIntFromTwoUshorts();
                    if (recordValuesQueryResult.Result.All((arg => arg == 0))) return Result<ushort[]>.Create(false);
                    if (readyMark == 0) return recordValuesQueryResult.Result;
                }

                return Result<ushort[]>.Create(false);

            });
            return res;
        }

        public void Initialize(IJournalSequenceInitializingParameters parameters)
        {
            _oscilloscopeLoadingSequenceInitializingParameters = parameters as IOscilloscopeLoadingSequenceInitializingParameters;
        }
    }
}
