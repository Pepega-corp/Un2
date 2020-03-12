using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Infrastructure.Values.Base
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class FormattedValueBase : IFormattedValue
    {
        public abstract string StrongName { get; }
        [JsonProperty]
        public string Header { get; set; }
        public abstract string AsString();
        public abstract T Accept<T>(IValueVisitor<T> visitor);
    }
}