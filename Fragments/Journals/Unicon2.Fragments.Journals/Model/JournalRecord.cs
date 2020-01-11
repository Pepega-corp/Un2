using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Journals.Model
{
    [DataContract(Namespace = "JournalRecordNS")]
    public class JournalRecord : IJournalRecord
    {
        private bool _isLoaded;

        public JournalRecord()
        {
            FormattedValues = new List<IFormattedValue>();
        }

        [DataMember]
        public int NumberOfRecord { get; set; }
        [DataMember]
        public bool IsLoaded
        {
            get { return _isLoaded; }
            set { _isLoaded = value; }
        }
        //TODO: посмотреть сериализацию записей журнала, пишет какую-то херню,когда записывает этот лист
        //[DataMember(Name = nameof(FormattedValues))]
        public List<IFormattedValue> FormattedValues { get; set; }
    }
}
