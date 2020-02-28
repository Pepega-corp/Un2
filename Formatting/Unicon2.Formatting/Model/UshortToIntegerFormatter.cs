using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(IsReference = true, Namespace = "UshortToIntegerFormatterNS")]
    public class UshortToIntegerFormatter : UshortsFormatterBase
    {
        public override object Clone()
        {
            return new UshortToIntegerFormatter();
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitUshortToIntegerFormatter(this);
        }
    }
}