using System;
using System.Runtime.Serialization;
using System.Text;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Formatting.Model
{
    [DataContract(Namespace = "StringFormatter1251NS", IsReference = true)]
    public class StringFormatter1251 : UshortsFormatterBase
    {
        public override object Clone()
        {
            return new StringFormatter1251();
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitString1251Formatter(this);
        }
    }
}