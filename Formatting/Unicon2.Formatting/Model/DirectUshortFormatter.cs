using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class DirectUshortFormatter : UshortsFormatterBase
    {
        public override object Clone()
        {
            return new DirectUshortFormatter();
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitDirectUshortFormatter(this);
        }
    }
}
