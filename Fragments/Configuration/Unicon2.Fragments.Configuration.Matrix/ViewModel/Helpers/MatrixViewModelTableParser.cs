using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Presentation.Values.Editable;
using Unicon2.SharedResources.Behaviors;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel.Helpers
{
    public class MatrixViewModelTableParser
    {
        public ushort[] GetUshortsFromTable(DynamicDataTable table, IMatrixValue matrixValue)
        {

            var ushortsFromTable = matrixValue.MatrixTemplate.MatrixMemoryVariables.SelectMany((variable => GetUshortsFromVariable(variable, matrixValue, table)
                )).ToList();
            return ushortsFromTable.ToArray();
        }

        public ushort[] GetUshortsFromVariable(IMatrixMemoryVariable variable, IMatrixValue matrixValue, DynamicDataTable dynamicDataTable)
        {
            var indexOfVariable = matrixValue.MatrixTemplate.MatrixMemoryVariables.IndexOf(variable);
            var row = dynamicDataTable.Values[indexOfVariable];
            var boolArray = new bool[matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable];

            matrixValue.MatrixTemplate.ResultBitOptions.ForEach((option => boolArray[option.NumbersOfAssotiatedBits.First()]=(row[matrixValue.MatrixTemplate.ResultBitOptions.IndexOf(option)] as EditableBoolValueViewModel).BoolValueProperty ));

            var size=variable.StartAddressBit + matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable;
           
               var newArray=new bool[size];
                boolArray.CopyTo(newArray, variable.StartAddressBit);

                var numberOfUshorts = Math.Ceiling((double)boolArray.Length / 16);

                var ushorts=new List<ushort>();
                for (int i = 0; i < numberOfUshorts; i++)
                {
                    var ushortBools = boolArray.Skip(i*16).Take(16).ToArray();
                    int resUshort = 0;
                    for (int j = 0; j < ushortBools.Count(); ++j)
                        if (ushortBools[j]) resUshort |= 1 << j;
                    ushorts.Add((ushort)resUshort);
                }

                return ushorts.ToArray();

        }
    }
}