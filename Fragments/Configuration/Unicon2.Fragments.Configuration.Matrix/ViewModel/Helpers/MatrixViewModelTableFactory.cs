using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Matrix.Model;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Editable;
using Unicon2.SharedResources.Behaviors;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel.Helpers
{
    public class MatrixViewModelTableFactory
    {
        private IMatrixValue _matrixValue;
        private readonly Func<IBoolValue> _boolValue;
        private readonly Func<IChosenFromListValue> _chosenFromListValFunc;
        private Func<IFormattedValueViewModel<IFormattedValue>> _formattedValueViewModelFunc;

        public MatrixViewModelTableFactory(Func<IBoolValue> boolValue, Func<IChosenFromListValue> chosenFromListValFunc)
        {
            _boolValue = boolValue;
            _chosenFromListValFunc = chosenFromListValFunc;
        }


        /* public DynamicDataTable CreateMatrixDataTable(IMatrixValue matrixValue, bool isEditable)
         {
             _matrixValue = matrixValue;
             switch (matrixValue.MatrixTemplate.MatrixVariableOptionTemplate)
             {
                 case ListMatrixVariableOptionTemplate _:
                 {
                     DynamicDataTable table = new DynamicDataTable(
                         matrixValue.MatrixTemplate.VariableColumnSignatures.Select((option => option.Signature))
                             .ToList(),
                         matrixValue.MatrixTemplate.MatrixMemoryVariables.Select((variable => variable.Name)).ToList(),
                         true);
 
                     if (isEditable)
                         _formattedValueViewModelFunc = () => new EditableChosenFromListValueViewModel();
                     else
                         _formattedValueViewModelFunc = () => new ChosenFromListValueViewModel();
 
                     _matrixValue.MatrixTemplate.MatrixMemoryVariables.ForEach((variable) =>
                     {
 
                         var bitArrayOfVariable = GetBitArrayOfVariable(variable);
                         table.AddFormattedValueViewModel(GetFormattedValueViewModels((signature =>
                             GetListCellViewModel(signature, bitArrayOfVariable))));
                     });
                     return table;
                 }
 
                 case BoolMatrixVariableOptionTemplate _:
                 {
                     DynamicDataTable table = new DynamicDataTable(
                         matrixValue.MatrixTemplate.ResultBitOptions.Select((option => option.FullSignature)).ToList(),
                         matrixValue.MatrixTemplate.MatrixMemoryVariables.Select((variable => variable.Name)).ToList(),
                         true);
                     if (isEditable)
                         _formattedValueViewModelFunc = () => new EditableBoolValueViewModel();
                     else
                         _formattedValueViewModelFunc = () => new BoolValueViewModel();
                     _matrixValue.MatrixTemplate.MatrixMemoryVariables.ForEach((variable) =>
                     {
 
                         var bitArrayOfVariable = GetBitArrayOfVariable(variable);
                         table.AddFormattedValueViewModel(GetFormattedValueViewModels((signature =>
                             GetBoolCellViewModel(signature, bitArrayOfVariable))));
                     });
                     return table;
                 }
 
 
             }
 
             return null;
         }
 
         private List<IFormattedValueViewModel> GetFormattedValueViewModels(
             Func<IVariableColumnSignature, IFormattedValueViewModel> cellViewModelGetFunc)
         {
             return MapVariableToValueViewModels(cellViewModelGetFunc);
         }
 
         private IFormattedValueViewModel GetListCellViewModel(IVariableColumnSignature signature,
             List<bool> bitArrayOfVariable)
         {
             try
             {
                 var matrixValue = _matrixValue;
                 IChosenFromListValue chosenFromListValue = _chosenFromListValFunc();
                 var optionsTemplate =
                     _matrixValue.MatrixTemplate.MatrixVariableOptionTemplate as ListMatrixVariableOptionTemplate;
 
                 var listToInit = optionsTemplate.OptionPossibleValues.Select((value => value.PossibleValueName))
                     .ToList();
                 listToInit.Add("нет");
                 chosenFromListValue.InitList(listToInit);
                 chosenFromListValue.SelectedItem = "нет";
                 var optionsList = matrixValue.MatrixTemplate.ResultBitOptions.Cast<ListMatrixBitOption>();
 
                 foreach (var optionInListbox in optionsTemplate.OptionPossibleValues)
                 {
                     var relatedOption = optionsList.First((option) =>
                         option.FullSignature == signature.Signature + " " + optionInListbox.PossibleValueName);
 
                     if (GetIsListItemSelected(relatedOption, bitArrayOfVariable, optionsList))
                         chosenFromListValue.SelectedItem =
                             chosenFromListValue.AvailableItemsList.First((item) =>
                                 item == optionInListbox.PossibleValueName);
                 }
 
                 var viewModel = _formattedValueViewModelFunc();
                 viewModel.InitFromValue(chosenFromListValue);
                 return viewModel;
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
                 throw;
             }
 
         }
 
 
         private bool GetIsListItemSelected(ListMatrixBitOption relatedOption, List<bool> bitArrayOfVariable,
             IEnumerable<ListMatrixBitOption> optionsList)
         {
             if (!(relatedOption.NumbersOfAssotiatedBits.Any() &&
                   bitArrayOfVariable[relatedOption.NumbersOfAssotiatedBits.First()]))
             {
                 return false;
             }
 
             if (relatedOption.OptionPossibleValue.PossibleValueConditions.Count == 0) return true;
 
             var condition = relatedOption
                 .OptionPossibleValue.PossibleValueConditions.First();
             var affectingOption = optionsList.First(option =>
                 option.FullSignature == relatedOption.VariableColumnSignature.Signature + " " +
                 condition.RelatedOptionPossibleValue.PossibleValueName);
             return affectingOption.NumbersOfAssotiatedBits.Any() &&
                    bitArrayOfVariable[affectingOption.NumbersOfAssotiatedBits.First()] &&
                    condition.BoolConditionRule;
         }
 
 
 
 
         private List<IFormattedValueViewModel> MapVariableToValueViewModels(
             Func<IVariableColumnSignature, IFormattedValueViewModel> cellViewModelGetFunc)
         {
             var matrixValue = _matrixValue;
             return matrixValue.MatrixTemplate.VariableColumnSignatures.Select((cellViewModelGetFunc)).ToList();
         }
 
         private IFormattedValueViewModel GetBoolCellViewModel(IVariableColumnSignature signature,
             List<bool> bitArrayOfVariable)
         {
             var matrixValue = _matrixValue;
             IBoolValue boolValue = _boolValue();
             try
             {
                 var resultBitOptionsWithSignature = matrixValue.MatrixTemplate.ResultBitOptions
                     .FirstOrDefault((option => option.VariableColumnSignature == signature));
                 if (resultBitOptionsWithSignature != null)
                 {
                     boolValue.BoolValueProperty =
                         bitArrayOfVariable[resultBitOptionsWithSignature.NumbersOfAssotiatedBits.First()];
                 }
           
             }
             catch (Exception e)
             {
                 Console.WriteLine(e);
             }
            
             var viewModel = _formattedValueViewModelFunc();
             viewModel.InitFromValue(boolValue);
             return viewModel;
         }
 
         private List<bool> GetBitArrayOfVariable(IMatrixMemoryVariable variable)
         {
             var bools = new List<bool>();
             GetVariableUshorts(variable).ForEach(arg =>
             {
                 var bitArray = new BitArray(new[] {(int) arg});
                 for (int i = 0; i < 16; i++)
                 {
                     bools.Add(bitArray[i]);
                 }
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
             return (int) Math.Ceiling(
                 (double) (variable.StartAddressBit + _matrixValue.MatrixTemplate.NumberOfBitsOnEachVariable) / 16);
         }*/
    }


}
