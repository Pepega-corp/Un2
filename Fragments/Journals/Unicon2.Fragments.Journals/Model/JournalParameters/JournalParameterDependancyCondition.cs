using System.Collections;
using System.Runtime.Serialization;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Journals.Infrastructure.Model.JournalParameters;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Dependancy;

namespace Unicon2.Fragments.Journals.Model.JournalParameters
{
    [DataContract(Namespace = "JournalParameterDependancyConditionNS")]
    public class JournalParameterDependancyCondition : IJournalCondition
    {
        #region Implementation of IJournalParameterDependancyCondition
        [DataMember]
        public ConditionsEnum ConditionsEnum { get; set; }
        [DataMember]
        public ushort UshortValueToCompare { get; set; }
        [DataMember]
        public IUshortsFormatter UshortsFormatter { get; set; }
        [DataMember]
        public IJournalParameter BaseJournalParameter { get; set; }

        public bool GetConditionResult(ushort[] recordUshorts)
        {
            if (ConditionsEnum == ConditionsEnum.HaveTrueBitAt)
            {
                BitArray bitArray = new BitArray(new int[] { recordUshorts[BaseJournalParameter.StartAddress] });
                if (BaseJournalParameter is ISubJournalParameter)
                {
                    bitArray = new BitArray(new int[] { (BaseJournalParameter as ISubJournalParameter).GetParameterUshortInRecord(recordUshorts) });
                }
                return bitArray[UshortValueToCompare];
            }
            if (ConditionsEnum == ConditionsEnum.HaveFalseBitAt)
            {
                BitArray bitArray = new BitArray(new int[] { recordUshorts[BaseJournalParameter.StartAddress] });
                if (BaseJournalParameter is ISubJournalParameter)
                {
                    bitArray = new BitArray(new int[] { (BaseJournalParameter as ISubJournalParameter).GetParameterUshortInRecord(recordUshorts) });
                }
                return !bitArray[UshortValueToCompare];
            }
            if (ConditionsEnum == ConditionsEnum.Equal)
            {
                ushort resUshort = recordUshorts[BaseJournalParameter.StartAddress];
                if (BaseJournalParameter is ISubJournalParameter)
                {
                    resUshort = (BaseJournalParameter as ISubJournalParameter).GetParameterUshortInRecord(recordUshorts);
                }
                return resUshort == UshortValueToCompare;
            }
            return false;
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => nameof(JournalParameterDependancyCondition);

        #endregion

        #region Implementation of INameable
        [DataMember]
        public string Name { get; set; }

        #endregion
    }
}
