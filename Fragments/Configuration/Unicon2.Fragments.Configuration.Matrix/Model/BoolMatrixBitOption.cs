using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(BoolMatrixBitOption), IsReference = true)]

    public class BoolMatrixBitOption : IBitOption
    {
        public BoolMatrixBitOption()
        {
            this.NumbersOfAssotiatedBits = new List<int>();
        }

        public string FullSignature => this.VariableColumnSignature?.Signature;
        [DataMember]
        public IVariableColumnSignature VariableColumnSignature { get; set; }
        [DataMember]
        public List<int> NumbersOfAssotiatedBits { get; set; }

        public bool IsBitOptionEqual(IBitOption comparingBitOption)
        {
            return comparingBitOption.VariableColumnSignature == this.VariableColumnSignature && (comparingBitOption.StrongName == this.StrongName);
        }

        public string StrongName => MatrixKeys.BOOL_MATRIX_BIT_OPTION;
    }
}
