﻿using System;
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


        public void FillMatrixDataTable(DynamicDataTable dataTable,
            Func<IFormattedValueViewModel> valueViewModelFunc)
        {
            _matrixValue.MatrixTemplate.MatrixMemoryVariables.ForEach((variable) => dataTable.AddFormattedValueViewModel(GetFormattedValueViewModels(variable, valueViewModelFunc)));
        }

        private List<IFormattedValueViewModel> GetFormattedValueViewModels(IMatrixMemoryVariable matrixMemoryVariable,
            Func<IFormattedValueViewModel> valueViewModelFunc)
        {
            Func<IVariableColumnSignature, IFormattedValueViewModel> cellGettingFunc = (signature) =>
                GetCellViewModel(valueViewModelFunc, matrixMemoryVariable,
                    signature);

            return MapVariableToValueViewModels(cellGettingFunc);
        }


        private List<IFormattedValueViewModel> MapVariableToValueViewModels(
            Func<IVariableColumnSignature, IFormattedValueViewModel> cellViewModelGetFunc)
        {
            var matrixValue = _matrixValue;
            return matrixValue.MatrixTemplate.VariableColumnSignatures.Select((cellViewModelGetFunc)).ToList();
        }

        private IFormattedValueViewModel GetCellViewModel(Func<IFormattedValueViewModel> boolValueFunc, IMatrixMemoryVariable variable, IVariableColumnSignature signature)
        {
            var matrixValue = _matrixValue;
            IBoolValue boolValue = _boolValue();
            boolValue.BoolValueProperty =
                GetBitArrayOfVariable(variable)[
                    matrixValue.MatrixTemplate.ResultBitOptions
                        .FirstOrDefault((option => option.VariableColumnSignature == signature))
                        .NumbersOfAssotiatedBits.First()];
            var viewModel = boolValueFunc();
            viewModel.InitFromValue(boolValue);
            return viewModel;
        }

        private List<bool> GetBitArrayOfVariable(IMatrixMemoryVariable variable)
        {
            var bools = new List<bool>();
            GetVariableUshorts(variable).ForEach(arg =>
            {
                foreach (bool o in new BitArray(new[] { (int)arg }))
                {
                    bools.Add(o);
                };
            });
            var matrixValue = _matrixValue;
            return bools.Take(matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable).ToList();
        }

        private ushort[] GetVariableUshorts(IMatrixMemoryVariable targetVariable)
        {
            var matrixValue = _matrixValue;
            var indexOfTargetVar = matrixValue.MatrixTemplate.MatrixMemoryVariables.IndexOf(targetVariable);
            var offset = matrixValue.MatrixTemplate.MatrixMemoryVariables
                .Where((variable =>
                    matrixValue.MatrixTemplate.MatrixMemoryVariables.IndexOf(variable) < indexOfTargetVar))
                .Sum(GetMatrixVariableOffset);

            return matrixValue.UshortsValue.Skip(offset)
                .Take(GetMatrixVariableOffset(targetVariable)).ToArray();
        }

        private int GetMatrixVariableOffset(IMatrixMemoryVariable variable)
        {
            return (int)Math.Ceiling((double)(variable.StartAddressBit + _matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable) / 16);
        }

    }
}