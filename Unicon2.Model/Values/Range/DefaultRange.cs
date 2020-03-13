using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Model.Values.Range
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DefaultRange : IRange
    {
        [JsonProperty]
        public double RangeFrom { get; set; }
        [JsonProperty]
        public double RangeTo { get; set; }
        public bool CheckValue(double valueToCheck)
        {
            if (valueToCheck > RangeTo) return false;
            if (valueToCheck < RangeFrom) return false;
            return true;
        }

        public bool CheckNesting(IRange range)
        {
            return (RangeFrom <= range.RangeFrom) && (RangeTo >= range.RangeTo);
        }

        public object Clone()
        {
            return new DefaultRange()
            {
                RangeFrom = RangeFrom,
                RangeTo = RangeTo
            };
        }
    }
}
