using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class NumericValue:FormattedValueBase,INumericValue
    {
        [DataMember]
        public string MeasureUnit { get; set; }
        [DataMember]
        public bool IsMeasureUnitEnabled { get; set; }

        public override string StrongName => nameof(NumericValue);
        public override string AsString()
        {
            return this.ToString();
        }

        [DataMember]
        public double NumValue { get; set; }

        public override string ToString()
        {
            return this.NumValue.ToString();
        }
    }
}
