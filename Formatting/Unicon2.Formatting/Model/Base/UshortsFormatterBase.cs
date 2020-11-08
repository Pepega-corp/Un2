using Newtonsoft.Json;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model.Base
{
    [JsonObject(MemberSerialization.OptIn)]
    public abstract class UshortsFormatterBase : Disposable, IUshortsFormatter
    {
        public abstract object Clone();

        [JsonProperty]
        public string Name { get; set; }

        public abstract T Accept<T>(IFormatterVisitor<T> visitor);
    }

}