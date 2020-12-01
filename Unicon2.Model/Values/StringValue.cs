using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]

    public class StringValue : FormattedValueBase, IStringValue
    {
        public override string StrongName => nameof(StringValue);

        public override string AsString()
        {
            return StrValue;
        }

        [JsonProperty] public string StrValue { get; set; }

        public override T Accept<T>(IValueVisitor<T> visitor)
        {
            return visitor.VisitStringValue(this);
        }
    }
}