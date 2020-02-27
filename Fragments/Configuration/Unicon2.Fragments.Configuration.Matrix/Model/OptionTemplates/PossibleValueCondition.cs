using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(PossibleValueCondition), IsReference = true)]

    public class PossibleValueCondition : IPossibleValueCondition
    {
        [DataMember]
        public bool BoolConditionRule { get; set; }
        [DataMember]
        public IOptionPossibleValue RelatedOptionPossibleValue { get; set; }
    }
}
