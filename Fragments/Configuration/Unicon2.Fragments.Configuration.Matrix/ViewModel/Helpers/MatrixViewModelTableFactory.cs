using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.SharedResources.Behaviors;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel.Helpers
{
   public class MatrixViewModelTableFactory
    {
        private readonly IMatrixValue _matrixValue;
        private Func<IBoolValue> _boolValue;

        public MatrixViewModelTableFactory(IMatrixValue matrixValue, Func<IBoolValue> boolValue)
        {
            _matrixValue = matrixValue;
            _boolValue = boolValue;
        } 


        public void FillMatrixDataTable(DynamicDataTable dataTable, IMatrixValue matrixValue,
            Func<IFormattedValueViewModel> valueViewModelFunc)
        {
            matrixValue.MatrixTemplate.MatrixMemoryVariables.ForEach((variable =>
            {
                dataTable.AddFormattedValueViewModel(
                    MapVariableToValueViewModels((signature) => GetCellViewModel(GetBitArrayOfVariable(GetVariableUshorts()), valueViewModelFunc).Invoke(variable, signature)).Invoke(variable));
            }));
        }




        private Func<IMatrixMemoryVariable, List<IFormattedValueViewModel>> MapVariableToValueViewModels(Func<IVariableColumnSignature, IFormattedValueViewModel> cellViewModelGetFunc)
        {
            return (variable) =>
            {
                var matrixValue = _matrixValue;
                if (matrixValue.MatrixTemplate.MatrixVariableOptionTemplate is BoolMatrixVariableOptionTemplate)
                {
                    return matrixValue.MatrixTemplate.VariableColumnSignatures.Select((cellViewModelGetFunc)).ToList();
                }
                throw new NotImplementedException();
            };
        }

        private Func<IMatrixMemoryVariable, IVariableColumnSignature, IFormattedValueViewModel> GetCellViewModel(Func<IMatrixMemoryVariable, List<bool>> bitArrayGetFunc, Func<IFormattedValueViewModel> boolValueFunc)
        {
            return (variable, signature) =>
            {
                var matrixValue = _matrixValue;
                IBoolValue boolValue = _boolValue();
                boolValue.BoolValueProperty =
                    bitArrayGetFunc(variable)[
                        matrixValue.MatrixTemplate.ResultBitOptions
                            .FirstOrDefault((option => option.VariableColumnSignature == signature))
                            .NumbersOfAssotiatedBits.First()];
                var viewModel = boolValueFunc();
                viewModel.InitFromValue(boolValue);
                return viewModel;
            };
        }

        private Func<IMatrixMemoryVariable, List<bool>> GetBitArrayOfVariable(Func<IMatrixMemoryVariable, ushort[]> variableUshortsGetFunc)
        {
            return (variable) =>
            {
                var bools = new List<bool>();
                variableUshortsGetFunc(variable).ForEach(arg =>
                {
                    foreach (bool o in new BitArray(new[] { (int)arg }))
                    {
                        bools.Add(o);
                    };
                });
                var matrixValue = _matrixValue;
                return bools.Take(matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable).ToList();
            };
        }

        private Func<IMatrixMemoryVariable, ushort[]> GetVariableUshorts()
        {
            return (targetVariable) =>
            {
                var matrixValue = _matrixValue;
                var indexOfTargetVar = matrixValue.MatrixTemplate.MatrixMemoryVariables.IndexOf(targetVariable);
                var offset = matrixValue.MatrixTemplate.MatrixMemoryVariables
                    .Where((variable =>
                        matrixValue.MatrixTemplate.MatrixMemoryVariables.IndexOf(variable) < indexOfTargetVar))
                    .Sum(GetMatrixVariableOffset);

                return matrixValue.UshortsValue.Skip(offset)
                    .Take(GetMatrixVariableOffset(targetVariable)).ToArray();
            };
        }

        private int GetMatrixVariableOffset(IMatrixMemoryVariable variable)
        {
            return (variable.StartAddressBit + _matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable) / 16;
        }

    }
}
