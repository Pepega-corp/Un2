using Newtonsoft.Json;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]

    public class BoolFormatter : UshortsFormatterBase, IBoolFormatter
    {

        public override object Clone()
        {
            return new BoolFormatter();
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitBoolFormatter(this);
        }
    }
}
