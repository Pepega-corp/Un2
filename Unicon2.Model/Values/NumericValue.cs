using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class NumericValue:FormattedValueBase,INumericValue
    {
        #region Implementation of IMeasurable
        [DataMember]
        public string MeasureUnit { get; set; }
        [DataMember]
        public bool IsMeasureUnitEnabled { get; set; }

        #endregion

        #region Implementation of IStronglyNamed

        public override string StrongName => nameof(NumericValue);
        public override string AsString()
        {
            return this.ToString();
        }

        #endregion

        #region Implementation of INumericValue
        [DataMember]
        public double NumValue { get; set; }

        #endregion

        #region Overrides of Object

        public override string ToString()
        {
            return this.NumValue.ToString();
        }

        #endregion
    }
}
