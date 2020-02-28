using System;
using System.Runtime.Serialization;
using Unicon2.Formatting.Infrastructure.Keys;
using Unicon2.Formatting.Infrastructure.Model;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Name = nameof(BoolFormatter), Namespace = "BoolFormatterNS", IsReference = true)]

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
