using System.Collections.Generic;

namespace Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.Helpers
{
    public interface IResultBitOptionSeedingStrategy
    {
        List<IBitOption> UpdateBitOptions(IMatrixTemplate matrixTemplate, List<IBitOption> existingBitOptions);
    }
}