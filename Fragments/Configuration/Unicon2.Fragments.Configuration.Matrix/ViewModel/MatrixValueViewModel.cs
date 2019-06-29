using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.ViewModel;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Base;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel
{
   public class MatrixValueViewModel: FormattableValueViewModelBase,IMatrixValueViewModel
    {
        private readonly Func<IBoolValue> _boolValue;
        private DynamicDataTable _table;

        #region Overrides of FormattableValueViewModelBase

        public MatrixValueViewModel(Func<IBoolValue> boolValue)
        {
            _boolValue = boolValue;
        }


        public override string StrongName => MatrixKeys.MATRIX_VALUE+ ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public override void InitFromValue(IFormattedValue value)
        {
            Model = value;
            FillTable();
        }

        private void FillTable()
        {
            IMatrixValue matrixValue=Model as IMatrixValue;
            if(matrixValue==null)return;
            try
            {
                Table = new DynamicDataTable(matrixValue.MatrixTemplate.ResultBitOptions.Select((option => option.FullSignature)).ToList(),
                    matrixValue.MatrixTemplate.MatrixMemoryVariables.Select((variable =>variable.Name )).ToList(), true);

              
              new MatrixViewModelTableFactory(matrixValue,_boolValue).FillMatrixDataTable(Table,()=>new BoolValueViewModel());

              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

       
      
           
        


        public DynamicDataTable Table
        {
            get => _table;
            set
            {
                _table = value;
                this.RaisePropertyChanged();
            }
        }
        
        #endregion

        #region Implementation of IViewModel

        public object Model { get; set; }

        #endregion
    }
}
