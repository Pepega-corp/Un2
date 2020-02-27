using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Base;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Visitors;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    public class MatrixValue: FormattedValueBase,IMatrixValue
    {
        public override string StrongName => MatrixKeys.MATRIX_VALUE;
        public override string AsString()
        {
            return "Matrix";
        }
        public override T Accept<T>(IValueVisitor<T> visitor)
        {
	        return visitor.VisitMatrixValue(this);
        }
		public IMatrixTemplate MatrixTemplate { get; set; }
    }
}