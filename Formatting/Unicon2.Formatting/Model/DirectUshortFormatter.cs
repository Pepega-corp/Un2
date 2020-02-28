using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "DirectUshortFormatterNS", IsReference = true)]
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
