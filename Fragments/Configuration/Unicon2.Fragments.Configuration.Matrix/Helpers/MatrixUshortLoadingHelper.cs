using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.Configuration.Matrix.Helpers
{
    public class MatrixUshortLoadingHelper
    {
        public static async Task FillMatrixUshorts(IAppointableMatrix appointableMatrix,IDataProvider dataProvider)
        {
           
                var matrixUshorts = new List<ushort>();
                foreach (var memoryVariable in appointableMatrix.MatrixTemplate.MatrixMemoryVariables)
                {
                    var sizeOfMemoryVariableInBits = memoryVariable.StartAddressBit +
                                                     appointableMatrix.MatrixTemplate.NumberOfBitsOnEachVariable;
                    var result = await dataProvider.ReadHoldingResgistersAsync(memoryVariable.StartAddressWord,
                        (ushort) (Math.Ceiling((double) sizeOfMemoryVariableInBits / 16)),
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
        public static async Task<bool> WriteMatrixUshorts(IAppointableMatrix appointableMatrix, IDataProvider dataProvider,ushort[] ushortsToWrite)
        {
            var numOfUshortsToSkip = 0;
            var res = true;
            foreach (var memoryVariable in appointableMatrix.MatrixTemplate.MatrixMemoryVariables)
            {
                var sizeOfMemoryVariableInBits = memoryVariable.StartAddressBit +
                                                 appointableMatrix.MatrixTemplate.NumberOfBitsOnEachVariable;
                var numOfUshorts = (ushort) (Math.Ceiling((double) sizeOfMemoryVariableInBits / 16));
                
                var result = await dataProvider.WriteMultipleRegistersAsync(memoryVariable.StartAddressWord,ushortsToWrite.Skip(numOfUshortsToSkip).Take(numOfUshorts).ToArray() ,
                    "Write matrix variable" + appointableMatrix.Name);
                numOfUshortsToSkip += numOfUshorts;
                if (result.IsSuccessful) continue;
                res = false;
                break;
            }

            if (!res) return false;
            appointableMatrix.DeviceUshortsValue = ushortsToWrite;
            appointableMatrix.ConfigurationItemChangedAction?.Invoke();
            return true;

        }
    }
}
