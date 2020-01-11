using System;
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
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Values;
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
        private ushort[] _initialUshortsToCompare;
        private DynamicDataTable _table;
        private bool _isEditable = true;

        public EditableMatrixValueViewModel(MatrixViewModelTableFactory matrixViewModelTableFactory)
        {
            _matrixViewModelTableFactory = matrixViewModelTableFactory;
            MatrixUpdatedCommand = new RelayCommand(OnMatrixEdited);
            ClearAssignedSignals = new RelayCommand(OnClearAssignedSignals);
        }

        private void OnMatrixEdited()
        {
            var newUshorts = (new MatrixViewModelTableParser()).GetUshortsFromTable(Table, Model as IMatrixValue);
            if (!newUshorts.SequenceEqual(_initialUshortsToCompare))
            {
                ValueChangedAction?.Invoke(newUshorts);
                SetIsChangedProperty(nameof(_initialUshortsToCompare), _initialUshortsToCompare != newUshorts);

            }
        }
        private void OnClearAssignedSignals()
        {
            if (!(Model is IMatrixValue matrixValue)) return;
            try
            {
                for (int i = 0; i < matrixValue.UshortsValue.Count(); i++)
                {
                    matrixValue.UshortsValue[i] = 0;
                }
                Table = _matrixViewModelTableFactory.CreateMatrixDataTable(matrixValue, true);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }



        }

        public override string StrongName => ApplicationGlobalNames.CommonInjectionStrings.EDITABLE + MatrixKeys.MATRIX_VALUE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public override void InitFromValue(IFormattedValue value)
        {
            Model = value;
            FillTable();
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

        private void FillTable()
        {
            if (!(Model is IMatrixValue matrixValue)) return;
            try
            {
                Table = _matrixViewModelTableFactory.CreateMatrixDataTable(matrixValue, true);
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

        public override void SetBaseValueToCompare(ushort[] ushortsToCompare)
        {
            _initialUshortsToCompare = ushortsToCompare;
        }

        public override object Model { get; set; }

        protected override void OnDisposing()
        {
            if (Table != null) Table.TableUpdateAction = null;
            base.OnDisposing();
        }

        public object Clone()
        {
            IMatrixValueViewModel matrixItem = this.OnCloning();

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
    }
}
