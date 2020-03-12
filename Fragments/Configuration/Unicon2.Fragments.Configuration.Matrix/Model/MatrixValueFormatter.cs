using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Formatting.Model.Base;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Infrastructure.Interfaces.Visitors;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;

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