using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;

namespace Unicon2.Fragments.Configuration.Matrix.Converters
{
    public class MatrixTemplateSelectingConverter : IMultiValueConverter
    {
        private IEnumerable<IMatrixVariableOptionTemplateEditorViewModel> _matrixVariableOptionTemplateEditorViewModels;

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] == null) return false;
            this._matrixVariableOptionTemplateEditorViewModels =
                values[1] as IEnumerable<IMatrixVariableOptionTemplateEditorViewModel>;
            IMatrixVariableOptionTemplateEditorViewModel matrixVariableOptionTemplateEditorViewModel =
                values[0] as IMatrixVariableOptionTemplateEditorViewModel;
            if ((matrixVariableOptionTemplateEditorViewModel.Model as IMatrixVariableOptionTemplate).StrongName ==
                parameter.ToString())
            {
                return true;
            }

            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            if (value.Equals(true))
            {
                return new[]
                {
                    this._matrixVariableOptionTemplateEditorViewModels.First((model =>
                        (model.Model as IMatrixVariableOptionTemplate).StrongName == parameter.ToString()))
                };
            }
            return null;
        }
    }
}
