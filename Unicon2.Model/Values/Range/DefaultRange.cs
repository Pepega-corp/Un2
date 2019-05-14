using System.Runtime.Serialization;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.Values.Range
{
    [DataContract(Namespace = "DefaultRangeNS")]
    public class DefaultRange : IRange
    {
        #region Implementation of IRange
        [DataMember(Name = nameof(RangeFrom))]
        public double RangeFrom { get; set; }
        [DataMember(Name = nameof(RangeTo))]

        public double RangeTo { get; set; }
        public bool CheckValue(double valueToCheck)
        {
            if (valueToCheck > this.RangeTo) return false;
            if (valueToCheck < this.RangeFrom) return false;
            return true;
        }

        public bool CheckNesting(IRange range)
        {
            return (this.RangeFrom <= range.RangeFrom) && (this.RangeTo >= range.RangeTo);
        }

        #endregion

        #region Implementation of ICloneable

        public object Clone()
        {
            return new DefaultRange()
            {
                RangeFrom = this.RangeFrom,
                RangeTo = this.RangeTo
            };
        }

        #endregion
    }
}
