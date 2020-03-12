using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Matrix;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]

    public class BoolMatrixBitOption : IBitOption
    {
        public BoolMatrixBitOption()
        {
            this.NumbersOfAssotiatedBits = new List<int>();
        }

        public string FullSignature => this.VariableColumnSignature?.Signature;
        [JsonProperty]
        public IVariableColumnSignature VariableColumnSignature { get; set; }
        [JsonProperty]
        public List<int> NumbersOfAssotiatedBits { get; set; }

        public bool IsBitOptionEqual(IBitOption comparingBitOption)
        {
            return comparingBitOption.VariableColumnSignature == this.VariableColumnSignature && (comparingBitOption.StrongName == this.StrongName);
        }

        public string StrongName => MatrixKeys.BOOL_MATRIX_BIT_OPTION;
    }
}
