using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model
{
    public interface IMatrixTemplate
    {
        int NumberOfBitsOnEachVariable { get; set; }
        List<IMatrixMemoryVariable> MatrixMemoryVariables { get; set; }
        List<IVariableSignature> VariableOptionSignatures { get; set; }
        IMatrixVariableOptionTemplate MatrixVariableOptionTemplate { get; set; }
        void UpdateResultBitOptions();
        List<IBitOption> ResultBitOptions { get; set; }
    }

}