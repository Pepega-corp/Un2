using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Journals.ViewModel.Helpers
{
    public class JournalStructureHelper
    {
        public static bool IsJournalStructureSimilar(IUniconJournal uniconJournalBase, IUniconJournal uniconJournalToCompare)
        {
            return CheckRecordTemplateCorrespondsValues(uniconJournalBase.RecordTemplate, uniconJournalToCompare.JournalRecords) ;

        }

        private static bool CheckRecordTemplateCorrespondsValues(IRecordTemplate recordTemplate, List<IJournalRecord> journalRecords)
        {
            return journalRecords.All(record => CheckRecordByTemplate(recordTemplate, record));
        }


        private static bool CheckRecordByTemplate(IRecordTemplate recordTemplate, IJournalRecord journalRecord)
        {
            var recordTemplateCounter = 0;
            foreach (var journalParameter in recordTemplate.JournalParameters)
            {
                var res = CheckRecordByParameter(journalRecord, journalParameter, recordTemplateCounter);
                recordTemplateCounter = res.Item2;
                if (!res.Item1)
                {
                    return false;
                }
            }

            if (journalRecord.FormattedValues.Count != recordTemplateCounter)
            {
                return false;
            }
            return true;
        }
        private static (bool, int) CheckRecordByParameter(IJournalRecord journalRecord, IJournalParameter journalParameter, int recordTemplateCounter)
        {
            if (journalParameter is IComplexJournalParameter complexJournalParameter)
            {
                foreach (var childJournalParameter in complexJournalParameter.ChildJournalParameters)
                {
                    var res = CheckRecordByParameter(journalRecord, childJournalParameter, recordTemplateCounter);
                    if (!res.Item1)
                    {
                        return (false, res.Item2);
                    }
                    recordTemplateCounter = res.Item2;
                }
                return (true, recordTemplateCounter);
            }

            if (journalRecord.FormattedValues.Count <= recordTemplateCounter)
            {
                return (false, ++recordTemplateCounter);
            }
            else
            {
                return (true, ++recordTemplateCounter);
            }
        }
    }
}