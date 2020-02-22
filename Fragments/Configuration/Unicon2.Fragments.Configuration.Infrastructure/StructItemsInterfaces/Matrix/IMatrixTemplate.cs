using System;
using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix
{

    public interface IMatrixTemplate : ICloneable
    {
        int NumberOfBitsOnEachVariable { get; set; }
        List<IMatrixMemoryVariable> MatrixMemoryVariables { get; set; }
        List<IVariableColumnSignature> VariableColumnSignatures { get; set; }
        IMatrixVariableOptionTemplate MatrixVariableOptionTemplate { get; set; }
        List<IBitOption> ResultBitOptions { get; set; }
    }

}