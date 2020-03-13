using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]
    public class BoolValue : FormattedValueBase, IBoolValue
    {
        [JsonProperty] public bool BoolValueProperty { get; set; }

        public override string StrongName => nameof(BoolValue);

        public override string AsString()
        {
            return BoolValueProperty.ToString();
        }

        public override T Accept<T>(IValueVisitor<T> visitor)
        {
            return visitor.VisitBoolValue(this);
        }
    }
}
