using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Keys;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(BoolMatrixBitOption), IsReference = true)]

    public class BoolMatrixBitOption : IBitOption
    {
        #region Implementation of IBitOption

        public BoolMatrixBitOption()
        {
            this.NumbersOfAssotiatedBits = new List<int>();
        }

        public string FullSignature => this.VariableSignature?.Signature;
        [DataMember]
        public IVariableSignature VariableSignature { get; set; }
        [DataMember]
        public List<int> NumbersOfAssotiatedBits { get; set; }

        public bool IsBitOptionEqual(IBitOption comparingBitOption)
        {
            return comparingBitOption.VariableSignature == this.VariableSignature && (comparingBitOption.StrongName == this.StrongName);
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => MatrixKeys.BOOL_MATRIX_BIT_OPTION;

        #endregion
    }
}
