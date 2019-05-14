using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(OptionPossibleValue), IsReference = true)]

    public class OptionPossibleValue : IOptionPossibleValue
    {
        public OptionPossibleValue()
        {
            this.PossibleValueConditions = new List<IPossibleValueCondition>();
        }

        #region Implementation of IOptionPossibleValue
        [DataMember]
        public string PossibleValueName { get; set; }
        [DataMember]
        public List<IPossibleValueCondition> PossibleValueConditions { get; set; }

        #endregion
    }
}
