using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]
    public class BitMaskValue : FormattedValueBase, IBitMaskValue
    {
        public BitMaskValue()
        {
            this.BitSignatures = new List<string>();
            this.BitArray = new List<List<bool>>();
        }


        public override string StrongName => nameof(BitMaskValue);

        public override string AsString()
        {
            return nameof(BitMaskValue);
        }

        [DataMember] public List<List<bool>> BitArray { get; set; }
        [DataMember] public List<string> BitSignatures { get; set; }

        public List<bool> GetAllBits()
        {
            List<bool> allBools = new List<bool>();
            this.BitArray.ForEach((list => allBools.AddRange(list)));
            return allBools;
        }
    }
}