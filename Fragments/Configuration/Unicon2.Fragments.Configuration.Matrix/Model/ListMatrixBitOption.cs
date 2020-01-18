using System.Collections.Generic;
using System.Runtime.Serialization;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;

namespace Unicon2.Fragments.Configuration.Matrix.Model
{
    [DataContract(Namespace = "AppointableMatrixNS", Name = nameof(ListMatrixBitOption), IsReference = true)]

    public class ListMatrixBitOption : IBitOption
    {
        public ListMatrixBitOption()
        {
            this.NumbersOfAssotiatedBits = new List<int>();
        }


        public string StrongName => MatrixKeys.LIST_MATRIX_BIT_OPTION;

        public string FullSignature => this.VariableColumnSignature.Signature + " " + this.OptionPossibleValue.PossibleValueName;

        [DataMember]
        public IVariableColumnSignature VariableColumnSignature { get; set; }

        [DataMember]
        public List<int> NumbersOfAssotiatedBits { get; set; }

        public bool IsBitOptionEqual(IBitOption comparingBitOption)
        {

            if (!(comparingBitOption is ListMatrixBitOption)) return false;
                return (comparingBitOption.VariableColumnSignature == this.VariableColumnSignature) &&
                   (((ListMatrixBitOption) comparingBitOption).OptionPossibleValue == this.OptionPossibleValue) && (comparingBitOption.StrongName == this.StrongName);

        }

        [DataMember]
        public IOptionPossibleValue OptionPossibleValue { get; set; }
    }
}
