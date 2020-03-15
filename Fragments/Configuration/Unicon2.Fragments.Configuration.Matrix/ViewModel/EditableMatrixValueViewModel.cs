using System;
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
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Infrastructure.Visitors;
using Unicon2.Presentation.Values;
using Unicon2.Presentation.Values.Base;
using Unicon2.Presentation.Values.Editable;
using Unicon2.SharedResources.Behaviors;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Configuration.Matrix.ViewModel
{
    public class EditableMatrixValueViewModel : EditableValueViewModelBase, IMatrixValueViewModel
    {
        private readonly MatrixViewModelTableFactory _matrixViewModelTableFactory;
        private DynamicDataTable _table;
        private bool _isEditable = true;
        private IMatrixValue _matrix;
        private ushort[] _initialUshortsToCompare;

        public EditableMatrixValueViewModel(MatrixViewModelTableFactory matrixViewModelTableFactory)
        {
            _matrixViewModelTableFactory = matrixViewModelTableFactory;
            MatrixUpdatedCommand = new RelayCommand(OnMatrixEdited);
            ClearAssignedSignals = new RelayCommand(OnClearAssignedSignals);
        }

        private void OnMatrixEdited()
        {
            var newUshorts = (new MatrixViewModelTableParser()).GetUshortsFromTable(Table, _matrix);
            if (!newUshorts.SequenceEqual(_initialUshortsToCompare))
            {
                SetIsChangedProperty();
            }
        }

        private void OnClearAssignedSignals()
        {
            try
            {
              //  Table = _matrixViewModelTableFactory.CreateMatrixDataTable(_matrix, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE +
                                             MatrixKeys.MATRIX_VALUE +
                                             ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        //public override void InitFromValue(IMatrixValue value)
        //{
        //    _matrix = value;
        //    Header = value.Header;
        //    FillTable();
        //}

        public DynamicDataTable Table
        {
            get => _table;
            set
            {
                _table = value;
                RaisePropertyChanged();
            }
        }

        private void FillTable()
        {
            try
            {
            //    Table = _matrixViewModelTableFactory.CreateMatrixDataTable(_matrix, true);
             //   _initialUshortsToCompare = (new MatrixViewModelTableParser()).GetUshortsFromTable(Table, _matrix);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ICommand MatrixUpdatedCommand { get; }
        public ICommand ClearAssignedSignals { get; }

        public bool IsEditable
        {
            get { return _isEditable; }
            private set
            {
                _isEditable = value;
                RaisePropertyChanged();
            }
        }

        protected override void OnDisposing()
        {
            if (Table != null) Table.TableUpdateAction = null;
            base.OnDisposing();
        }

        public object Clone()
        {
            IMatrixValueViewModel matrixItem = OnCloning();

            return matrixItem;
        }

        private IMatrixValueViewModel OnCloning()
        {
            //EditableMatrixValueViewModel cloneModel = new EditableMatrixValueViewModel();
            //cloneModel.Model = this.Model;
            //cloneModel.IsEditEnabled = this.IsEditEnabled;
            //cloneModel.Header = this.Header;
            //cloneModel.Range = Range.Clone() as IRange;
            //cloneModel.IsRangeEnabled = this.IsRangeEnabled;
            //return cloneModel as IMatrixValueViewModel;
            return this;
        }

       
        public override T Accept<T>(IEditableValueViewModelVisitor<T> visitor)
        {
            return visitor.VisitMatrixViewModel(this);
        }
    }
}