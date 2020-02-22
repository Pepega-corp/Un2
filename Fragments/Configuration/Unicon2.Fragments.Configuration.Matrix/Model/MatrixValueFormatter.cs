using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Formatting.Model.Base;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Infrastructure.Values;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Name = nameof(MatrixValueFormatter), Namespace = "MatrixValueFormatterNS", IsReference = true)]
    public class MatrixValueFormatter: UshortsFormatterBase
    {
        private IAppointableMatrix _appointableMatrix;

        public MatrixValueFormatter(IAppointableMatrix appointableMatrix)
        {
            _appointableMatrix = appointableMatrix;
        }

        public override ushort[] FormatBack(IFormattedValue formattedValue)
        {
            throw new NotImplementedException();
        }

        protected override IFormattedValue OnFormatting(ushort[] ushorts)
        {
            return new MatrixValue(){MatrixTemplate=_appointableMatrix.MatrixTemplate,UshortsValue=ushorts};
        }

        public override string StrongName { get; }
        public override object Clone()
        {
            throw new NotImplementedException();
        }
    }
}
