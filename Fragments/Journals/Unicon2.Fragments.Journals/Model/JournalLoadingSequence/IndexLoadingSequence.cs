﻿using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Keys;
using Unicon2.Fragments.Journals.Infrastructure.Model.LoadingSequence;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Journals.Model.JournalLoadingSequence
{
    [DataContract(Namespace = "IndexLoadingSequence")]
    public class IndexLoadingSequence : IIndexLoadingSequence
    {
        private IDataProvider _dataProvider;
        private ushort _currentRecordIndex = 0;
        private int _lastQueryErrorCode = 1;// code "0" means last record

        #region Implementation of IDataProviderContaining

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        #endregion

        public void Initialize(IJournalSequenceInitializingParameters journalSequenceInitializingParameters)
        {

        }
        public async Task<bool> GetIsNextRecordAvailable()
        {
            return !(_lastQueryErrorCode == 0);
        }

        public async Task<ushort[]> GetNextRecordUshorts()
        {
            await _dataProvider.WriteSingleRegisterAsync(JournalStartAddress, _currentRecordIndex, "Write");
            this._currentRecordIndex++;

            IQueryResult<ushort[]> queryResult = await this._dataProvider.ReadHoldingResgistersAsync(JournalStartAddress,
                NumberOfPointsInRecord,
                JournalKeys.JOURNAL_RECORD_READING_QUERY);
            _lastQueryErrorCode = queryResult.Result[queryResult.Result.Length - 1];
            return queryResult.Result;
        }

        public void ResetSequence()
        {
            this._lastQueryErrorCode = 1;
        }

        #region Implementation of IOffsetLoadingSequence


        [DataMember]
        public ushort JournalStartAddress { get; set; }

        [DataMember]
        public ushort NumberOfPointsInRecord { get; set; }

        [DataMember]
        public ushort WordFormatFrom { get; set; }

        [DataMember]
        public ushort WordFormatTo { get; set; }

        [DataMember]
        public bool IsWordFormatNotForTheWholeRecord { get; set; }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => JournalKeys.INDEX_LOADING_SEQUENCE;

        #endregion

    }
}
