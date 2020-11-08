using Newtonsoft.Json;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Model.Values
{
    [JsonObject(MemberSerialization.OptIn)]

    public class ErrorValue : FormattedValueBase, IErrorValue
    {
        public override string StrongName => nameof(ErrorValue);

        public override string AsString()
        {
            return ErrorMessage;
        }

        public override T Accept<T>(IValueVisitor<T> visitor)
        {
            return visitor.VisitErrorValue(this);
        }

        [JsonProperty] public string ErrorMessage { get; set; }
    }
}