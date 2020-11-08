using System;
using Newtonsoft.Json;
using Unicon2.Formatting.Model.Base;
using Unicon2.Infrastructure.Interfaces.Visitors;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MatrixValueFormatter : UshortsFormatterBase
    {

        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override T Accept<T>(IFormatterVisitor<T> visitor)
        {
            return visitor.VisitMatrixFormatter(this);
        }
    }
}