using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [DataContract(Namespace = "ComplexJournalParameterNS", IsReference = true)]
    public class ComplexJournalParameter : JournalParameter, IComplexJournalParameter
    {
        public ComplexJournalParameter()
        {
            this.ChildJournalParameters = new List<ISubJournalParameter>();
        }

        [DataMember]
        public List<ISubJournalParameter> ChildJournalParameters { get; set; }

        public override void InitializeFromContainer(ITypesContainer container)
        {
            foreach (ISubJournalParameter subJournalParameter in this.ChildJournalParameters)
            {
                subJournalParameter.InitializeFromContainer(container);
            }
            base.InitializeFromContainer(container);
        }

        public override async Task<List<IFormattedValue>> GetFormattedValues(ushort[] recordUshorts)
        {
            List<IFormattedValue> formattedValues = new List<IFormattedValue>();
            foreach (ISubJournalParameter childJournalParameter in this.ChildJournalParameters)
            {
                formattedValues.Add(childJournalParameter.UshortsFormatter.Format(new ushort[] { childJournalParameter.GetParameterUshortInRecord(recordUshorts) }));
            }
            return formattedValues;
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext sc)
        {
            this.ChildJournalParameters.ForEach((parameter => parameter.ParentComplexJournalParameter = this));
        }
    }
}
