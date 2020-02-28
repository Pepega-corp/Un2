using System;
using System.Runtime.Serialization;
using System.Text;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "AsciiStringFormatterNS", IsReference = true)]
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
