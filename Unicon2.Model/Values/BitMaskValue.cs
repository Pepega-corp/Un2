using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BitMaskValue : FormattedValueBase, IBitMaskValue
    {
        public BitMaskValue()
        {
            BitSignatures = new List<string>();
            BitArray = new List<List<bool>>();
        }


        public override string StrongName => nameof(BitMaskValue);

        public override string AsString()
        {
            return nameof(BitMaskValue);
        }

        public override T Accept<T>(IValueVisitor<T> visitor)
        {
            return visitor.VisitBitMaskValue(this);
        }

        [JsonProperty] public List<List<bool>> BitArray { get; set; }
        [JsonProperty] public List<string> BitSignatures { get; set; }

        public List<bool> GetAllBits()
        {
            List<bool> allBools = new List<bool>();
            BitArray.ForEach((list => allBools.AddRange(list)));
            return allBools;
        }
    }
}