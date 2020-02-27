using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Unicon2.Fragments.Configuration.Exporter.Interfaces;
using Unicon2.Fragments.Configuration.Infrastructure.Export;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces;
using Unicon2.Fragments.Configuration.Matrix.Model;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.ViewModel.Helpers;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;
using Unicon2.Presentation.Values;
using Unicon2.SharedResources.Behaviors;

namespace Unicon2.Fragments.Configuration.Exporter.ItemRenderers
{
    public class MatrixRenderer : IConfigurationItemRenderer
    {
        private readonly MatrixViewModelTableFactory _matrixViewModelTableFactory;

        public MatrixRenderer(MatrixViewModelTableFactory matrixViewModelTableFactory)
        {
            _matrixViewModelTableFactory = matrixViewModelTableFactory;
        }

        private string GetSelectedBitOptionsForBoolMatrix(DynamicDataTable table, string rowHeader,
            IMatrixVariableOptionTemplate matrixTemplateMatrixVariableOptionTemplate)
        {
            if (matrixTemplateMatrixVariableOptionTemplate is ListMatrixVariableOptionTemplate)
            {
                return string.Join(", ", table.Values[table.RowHeadersStrings.IndexOf(rowHeader)]
                    .Where(value => (value as ChosenFromListValueViewModel).SelectedItem!="нет")
                    .Select(value =>$"{table.ColumnNamesStrings[table.Values[table.RowHeadersStrings.IndexOf(rowHeader)].IndexOf(value)]} {(value as ChosenFromListValueViewModel).SelectedItem}"));
            }
            else
            {
                 return string.Join(", ", table.Values[table.RowHeadersStrings.IndexOf(rowHeader)]
                .Where(value => (value as BoolValueViewModel).BoolValueProperty)
                .Select(value =>
                    table.ColumnNamesStrings[
                        table.Values[table.RowHeadersStrings.IndexOf(rowHeader)].IndexOf(value)]));
            }
          
        }

        private Result<string> GetMatrixValueString(AppointableMatrix matrix, ushort[] ushortValue)
        {
            if(ushortValue==null)return Result<string>.Create(false);
            if(matrix.UshortsFormatter==null) return Result<string>.Create(false);
            var matrixValue = matrix.UshortsFormatter.Format(ushortValue);
            var table = _matrixViewModelTableFactory.CreateMatrixDataTable(matrixValue as IMatrixValue, false);
            return Result<string>.Create(string.Join("<br/>", table.RowHeadersStrings
                .Select(rowHeader =>
                    $"{rowHeader}: {GetSelectedBitOptionsForBoolMatrix(table, rowHeader, matrix.MatrixTemplate.MatrixVariableOptionTemplate)}")),true);
        }

        public Maybe<List<TagBuilder>> RenderHtmlFromItem(IConfigurationItem configurationItem,
            SelectorForItemsGroup selectorForItemsGroup,
            int depthLevel = 0)
        {
            var matrix = configurationItem as AppointableMatrix;
            var tagBuilder = ConfigTableRowRenderer
                .Create()
                .SetName(new RenderData("", depthLevel == 0 ? "rootItem" : null))
                .SetDepth(depthLevel)
                .SetShouldRenderEmptyItems(depthLevel != 0)
                .SetDeviceData(GetMatrixValueString(matrix,matrix.DeviceUshortsValue))
                .SetLocalData(GetMatrixValueString(matrix, matrix.LocalUshortsValue))
                .SetSelectors(selectorForItemsGroup.IsPrintDeviceValuesAllowed,
                    selectorForItemsGroup.IsPrintLocalValuesAllowed)
                .Render();
            return Maybe<List<TagBuilder>>.FromValue(new List<TagBuilder>() {tagBuilder});
        }

    }
}