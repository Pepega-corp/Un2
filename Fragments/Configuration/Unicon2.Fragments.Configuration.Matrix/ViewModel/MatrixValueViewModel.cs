using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.ViewModel;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.ViewModel.Helpers;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Base;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel
{
    public class MatrixValueViewModel : FormattableValueViewModelBase, IMatrixValueViewModel
    {
        private readonly MatrixViewModelTableFactory _matrixViewModelTableFactory;
        private DynamicDataTable _table;
        private bool _isEditable = false;

        public MatrixValueViewModel(MatrixViewModelTableFactory matrixViewModelTableFactory)
        {
            _matrixViewModelTableFactory = matrixViewModelTableFactory;
        }


        public override string StrongName => MatrixKeys.MATRIX_VALUE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public override string AsString()
        {
	        return "Matrix";
        }
        //public override void InitFromValue(IFormattedValue value)
        //{
        //    Model = value;
        //    FillTable();
        //    base.InitFromValue(value);
        //}

        //private void FillTable()
        //{
        //    IMatrixValue matrixValue = Model as IMatrixValue;
        //    if (matrixValue == null) return;
        //    try
        //    {
        //        Table = _matrixViewModelTableFactory.CreateMatrixDataTable(matrixValue, false);
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //        throw;
        //    }
        //}

        public object Clone()
        {
            IMatrixValueViewModel matrixItem = OnCloning();

            return matrixItem;
        }

        private IMatrixValueViewModel OnCloning()
        {
            //MatrixValueViewModel cloneModel = new MatrixValueViewModel();
            return this;
        }

        public bool IsEditable
        {
            get { return _isEditable; }
            private set
            {
                _isEditable = value;
                RaisePropertyChanged();
            }
        }





        public DynamicDataTable Table
        {
            get => _table;
            set
            {
                _table = value;
                RaisePropertyChanged();
            }
        }

        public object Model { get; set; }
    }
}
