using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ListMatrixBitOption : IBitOption
    {
        public ListMatrixBitOption()
        {
            NumbersOfAssotiatedBits = new List<int>();
        }


        public string StrongName => MatrixKeys.LIST_MATRIX_BIT_OPTION;

        public string FullSignature => VariableColumnSignature.Signature + " " + OptionPossibleValue.PossibleValueName;

        [JsonProperty]
        public IVariableColumnSignature VariableColumnSignature { get; set; }

        [JsonProperty]
        public List<int> NumbersOfAssotiatedBits { get; set; }

        public bool IsBitOptionEqual(IBitOption comparingBitOption)
        {

            if (!(comparingBitOption is ListMatrixBitOption)) return false;
                return (comparingBitOption.VariableColumnSignature == VariableColumnSignature) &&
                   (((ListMatrixBitOption) comparingBitOption).OptionPossibleValue == OptionPossibleValue) && (comparingBitOption.StrongName == StrongName);

        }

        [JsonProperty]
        public IOptionPossibleValue OptionPossibleValue { get; set; }
    }
}
