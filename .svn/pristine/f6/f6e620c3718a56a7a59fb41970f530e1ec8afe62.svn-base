using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Factories;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Factory
{
    public class JournalRecordFactory : IJournalRecordFactory
    {
        private readonly ITypesContainer _container;

        public JournalRecordFactory(ITypesContainer container)
        {
            this._container = container;
        }

        #region Implementation of IJournalRecordFactory

        public async Task<IJournalRecord> CreateJournalRecord(ushort[] values, IRecordTemplate recordTemplate)
        {
            IJournalRecord journalRecord = this._container.Resolve<IJournalRecord>();
            foreach (IJournalParameter journalParameter in recordTemplate.JournalParameters)
            {
                journalRecord.FormattedValues.AddRange(await journalParameter.GetFormattedValues(values));
            }
            return journalRecord;
        }

        #endregion
    }
}
