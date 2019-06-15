using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Values.Base;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel
{
    public class EditableMatrixValueViewModel : EditableValueViewModelBase
    {
        #region Overrides of EditableValueViewModelBase

        public EditableMatrixValueViewModel()
        {
            ShowDetails = new RelayCommand(OnShowDetails);
        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE + MatrixKeys.MATRIX_VALUE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public override void InitFromValue(IFormattedValue value)
        {
            Model = value;
        }

        private void OnShowDetails()
        {
            
        }

        public ICommand ShowDetails { get; }


        public override void SetBaseValueToCompare(ushort[] ushortsToCompare)
        {
            throw new NotImplementedException();
        }

        public override object Model { get; set; }

        #endregion
    }
}
