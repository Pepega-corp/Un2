using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Journals.Infrastructure.Model;

namespace Unicon2.Fragments.Journals.Model
{
    [DataContract(Namespace = "RecordTemplateNS",IsReference = true)]
    public class RecordTemplate : IRecordTemplate
    {
        public RecordTemplate()
        {
            JournalParameters = new List<IJournalParameter>();
        }


        #region Implementation of IRecordTemplate
        [DataMember]
        public List<IJournalParameter> JournalParameters { get; set; }

        #endregion


    }
}
