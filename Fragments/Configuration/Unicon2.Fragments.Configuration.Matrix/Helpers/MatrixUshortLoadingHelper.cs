using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Helpers
{
    public class MatrixUshortLoadingHelper
    {
        public static async Task FillMatrixUshorts(IAppointableMatrix appointableMatrix,IDataProvider dataProvider)
        {
            List<ushort> matrixUshorts=new List<ushort>();
            foreach (var memoryVariable in appointableMatrix.MatrixTemplate.MatrixMemoryVariables)
            {
                var sizeOfMemoryVariableInBits = memoryVariable.StartAddressBit +
                                                 appointableMatrix.MatrixTemplate.NumberOfBitsOnEachVariable;
              var result = await  dataProvider.ReadHoldingResgistersAsync(memoryVariable.StartAddressWord,(ushort)(sizeOfMemoryVariableInBits / 16),
                    "Read matrix variable" + appointableMatrix.Name);
                if (result.IsSuccessful)
                {
                    matrixUshorts.AddRange(result.Result);
                }
                else
                {
                    return;
                }
            }
            appointableMatrix.DeviceUshortsValue = matrixUshorts.ToArray();
        }
    }
}
