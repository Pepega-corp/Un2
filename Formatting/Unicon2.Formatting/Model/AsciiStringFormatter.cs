using Newtonsoft.Json;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AsciiStringFormatter : UshortsFormatterBase
    {
        public override object Clone()
        {
            return new AsciiStringFormatter();
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitAsciiStringFormatter(this);
        }
    }
}
