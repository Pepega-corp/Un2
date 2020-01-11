using System.Runtime.Serialization;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;

namespace Unicon2.Model.Values
{
    [DataContract(Namespace = "ValuesNS")]

    public class StringValue:FormattedValueBase, IStringValue
    {
        public override string StrongName => nameof(StringValue);
        public override string AsString()
        {
            return this.StrValue;
        }

        [DataMember]
        public string StrValue { get; set; }
    }
}
