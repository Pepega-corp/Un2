using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]
   public class BoolValue:FormattedValueBase,IBoolValue
    {
        [DataMember]
        public bool BoolValueProperty { get; set; }

        public override string StrongName => nameof(BoolValue);
        public override string AsString()
        {
            return this.BoolValueProperty.ToString();
        }
    }
}
