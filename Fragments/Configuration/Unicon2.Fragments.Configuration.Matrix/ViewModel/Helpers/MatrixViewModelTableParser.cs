using System;
using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Matrix.Model;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure.Values.Matrix;
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

        private ushort[] GetUshortsFromVariable(IMatrixMemoryVariable variable, IMatrixValue matrixValue, DynamicDataTable dynamicDataTable)
        {
            var indexOfVariable = matrixValue.MatrixTemplate.MatrixMemoryVariables.IndexOf(variable);
            var row = dynamicDataTable.Values[indexOfVariable];
            var boolArray = new bool[matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable];





            switch (matrixValue.MatrixTemplate.MatrixVariableOptionTemplate)
            {
                case BoolMatrixVariableOptionTemplate _:
                    matrixValue.MatrixTemplate.ResultBitOptions.ForEach((option =>
                        boolArray[option.NumbersOfAssotiatedBits.First()] =
                            (row[matrixValue.MatrixTemplate.ResultBitOptions.IndexOf(option)] as
                                EditableBoolValueViewModel)
                            .BoolValueProperty));
                    break;
                case ListMatrixVariableOptionTemplate _:
                    row.ForEach((valueViewModel =>
                    {
                        if (valueViewModel is EditableChosenFromListValueViewModel chosenFromListValueViewModel &&
                            chosenFromListValueViewModel.SelectedItem != "нет")
                        {
                            var indexOfValue = row.IndexOf(valueViewModel);
                            var signature = dynamicDataTable.ColumnNamesStrings[indexOfValue];
                            var optionSelected = matrixValue.MatrixTemplate.ResultBitOptions.FirstOrDefault((option =>
                                option.FullSignature == signature + " " + chosenFromListValueViewModel.SelectedItem));
                            ApplySelectedListValue((ListMatrixBitOption)optionSelected, boolArray, matrixValue);
                        }
                    }));
                    break;
            }



            var size = variable.StartAddressBit + matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable;

            var newArray = new bool[size];
            boolArray.CopyTo(newArray, variable.StartAddressBit);

            var numberOfUshorts = Math.Ceiling((double)boolArray.Length / 16);

            var ushorts = new List<ushort>();
            for (int i = 0; i < numberOfUshorts; i++)
            {
                var ushortBools = boolArray.Skip(i * 16).Take(16).ToArray();
                int resUshort = 0;
                for (int j = 0; j < ushortBools.Count(); ++j)
                    if (ushortBools[j]) resUshort |= 1 << j;
                ushorts.Add((ushort)resUshort);
            }

            return ushorts.ToArray();

        }

        private void ApplySelectedListValue(ListMatrixBitOption optionSelected, bool[] boolArray, IMatrixValue matrixValue)
        {
            if (optionSelected.OptionPossibleValue.PossibleValueConditions.Count > 0)
            {
                var condition = optionSelected.OptionPossibleValue.PossibleValueConditions.First();
                var affectingOption = matrixValue.MatrixTemplate.ResultBitOptions.First((option =>
                    option.FullSignature == optionSelected.VariableColumnSignature.Signature + " " +
                    condition.RelatedOptionPossibleValue.PossibleValueName));

                if (optionSelected.NumbersOfAssotiatedBits.Any())
                    boolArray[optionSelected.NumbersOfAssotiatedBits.First()] = true;

                if (affectingOption.NumbersOfAssotiatedBits.Any())
                    boolArray[affectingOption.NumbersOfAssotiatedBits.First()] = condition.BoolConditionRule;

            }
            else
            {
                if (optionSelected.NumbersOfAssotiatedBits.Any())
                    boolArray[optionSelected.NumbersOfAssotiatedBits.First()] = true;
            }
        }



    }
}