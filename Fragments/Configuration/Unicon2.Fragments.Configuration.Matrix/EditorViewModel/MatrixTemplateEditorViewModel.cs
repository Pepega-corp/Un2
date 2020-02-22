using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ControlzEx.Standard;
using MahApps.Metro.Controls.Dialogs;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.Helpers;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Validators;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Infrastructure.Services;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{

    public class MatrixTemplateEditorViewModel : ValidatableBindableBase, IMatrixTemplateEditorViewModel
    {
        private readonly IMatrixMemoryVariableEditorViewModelFactory _matrixMemoryVariableEditorViewModelFactory;
        private readonly IVariableSignatureEditorViewModelFactory _variableSignatureEditorViewModelFactory;
        private readonly IGeneralViewModelFactory<IMatrixVariableOptionTemplateEditorViewModel> _generalViewModelFactory;
        private readonly IMatrixVariableOptionTemplateEditorViewModelFactory _matrixVariableOptionTemplateEditorViewModelFactory;
        private readonly IGeneralViewModelFactory<IBitOptionEditorViewModel> _bitOptionFactory;
        private readonly IGeneralViewModelFactory<IAssignedBitEditorViewModel> _assignedBitViewModelFactory;
        private readonly IBitOptionUpdatingStrategy _bitOptionUpdatingStrategy;
        private readonly ILocalizerService _localizerService;
        private readonly IDialogCoordinator _dialogCoordinator;
        private IMatrixTemplate _model;
        private int _numberOfBitsOnEachVariable;
        private List<IMatrixVariableOptionTemplateEditorViewModel> _availableMatrixVariableOptionTemplateEditorViewModels;
        private IMatrixVariableOptionTemplateEditorViewModel _selectedMatrixVariableOptionTemplateEditorViewModel;
        private List<IAssignedBitEditorViewModel> _assignedBitEditorViewModels;
        private ObservableCollection<IBitOptionEditorViewModel> _bitOptionEditorViewModels;
        private string _matrixName;
        private IMatrixTemplate _unchangedMatrixTemplate;
        private object _selectedTab;

        private string _valueSignatureMask;
        private int _valueSignatureNumberOfPoints;
        private bool _isInversion;

        private const string _inversionString = " Инв";

        public MatrixTemplateEditorViewModel(
            IMatrixMemoryVariableEditorViewModelFactory matrixMemoryVariableEditorViewModelFactory,
            IVariableSignatureEditorViewModelFactory variableSignatureEditorViewModelFactory,
            IGeneralViewModelFactory<IMatrixVariableOptionTemplateEditorViewModel> generalViewModelFactory,
            IMatrixVariableOptionTemplateEditorViewModelFactory matrixVariableOptionTemplateEditorViewModelFactory,
            IGeneralViewModelFactory<IBitOptionEditorViewModel> bitOptionFactory,
            IGeneralViewModelFactory<IAssignedBitEditorViewModel> assignedBitViewModelFactory,
            IBitOptionUpdatingStrategy bitOptionUpdatingStrategy, ILocalizerService localizerService, IDialogCoordinator dialogCoordinator)
        {
            this._matrixMemoryVariableEditorViewModelFactory = matrixMemoryVariableEditorViewModelFactory;
            this._variableSignatureEditorViewModelFactory = variableSignatureEditorViewModelFactory;
            this._generalViewModelFactory = generalViewModelFactory;
            this._matrixVariableOptionTemplateEditorViewModelFactory = matrixVariableOptionTemplateEditorViewModelFactory;
            this._bitOptionFactory = bitOptionFactory;
            this._assignedBitViewModelFactory = assignedBitViewModelFactory;
            _bitOptionUpdatingStrategy = bitOptionUpdatingStrategy;
            _localizerService = localizerService;
            _dialogCoordinator = dialogCoordinator;
            this.MatrixMemoryVariableEditorViewModels = new ObservableCollection<IMatrixMemoryVariableEditorViewModel>();
            this.AddMatrixVariableCommand = new RelayCommand(this.OnAddMatrixVariableExucute);
            this.VariableSignatureEditorViewModels = new ObservableCollection<IVariableSignatureEditorViewModel>();
            this.AddSignatureCommand = new RelayCommand(this.OnAddSignatureExucute);
            this.DeleteMatrixVariableCommand = new RelayCommand<object>(this.OnDeleteMatrixVariableExecute);
            this.DeleteSignatureCommand = new RelayCommand<object>(this.OnDeleteSignatureExecute);
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmitExecute, CanExecuteSubmit);
            this.CancelCommand = new RelayCommand<object>(this.OnCancelExecute);
            this.AssignSignalsAutomatically = new RelayCommand(this.OnAssignSignalsAutomatically);
            this.ClearAssignedSignals = new RelayCommand(this.OnClearAssignedSignals);
            this.ClearSignaturesCommand = new RelayCommand(this.OnClearSignatures);
            this.AddSignatureGroupCommand = new RelayCommand(this.OnAddSignatureGroupExecute);
            this.AvailableMatrixVariableOptionTemplateEditorViewModels =
                this._matrixVariableOptionTemplateEditorViewModelFactory.CreateAvailableMatrixVariableOptionTemplateEditorViewModel();
            this._bitOptionEditorViewModels = new ObservableCollection<IBitOptionEditorViewModel>();
            this.AssignedBitEditorViewModels = new ObservableCollection<IAssignedBitEditorViewModel>();

            ValueSignatureMask = string.Empty;
            ValueSignatureNumberOfPoints = 0;
        }

        private void OnAssignSignalsAutomatically()
        {
            var minFromAsingableAndOptions = Math.Min(AssignedBitEditorViewModels.Count, _bitOptionEditorViewModels.Count);
            for (int i = 0; i < minFromAsingableAndOptions; i++)
            {
                AssignedBitEditorViewModels[i].SelectedBitOptionEditorViewModel = _bitOptionEditorViewModels[i];
            }

        }
        private void OnClearAssignedSignals()
        {
            var minFromAsingableAndOptions = Math.Min(AssignedBitEditorViewModels.Count, _bitOptionEditorViewModels.Count);
            for (int i = 0; i < minFromAsingableAndOptions; i++)
            {
                AssignedBitEditorViewModels[i].SelectedBitOptionEditorViewModel = null;
            }
        }
        private void OnClearSignatures()
        {
            VariableSignatureEditorViewModels.Clear();
        }
        private bool CanExecuteSubmit(object obj)
        {
            return !HasErrors;
        }


        public object SelectedTab
        {
            get => _selectedTab;
            set
            {
                FireErrorsChanged();
                if (!HasErrors)
                {
                    _selectedTab = value;
                    this.SaveChanges();
                    this.UpdateViewModel();
                }

                RaisePropertyChanged();
            }
        }

        private void UpdateViewModel()
        {
            _bitOptionUpdatingStrategy.UpdateBitOptions(_model);
            //   this._model.UpdateResultBitOptions();
            this.AssignedBitEditorViewModels.Clear();
            this.BitOptionEditorViewModels = new ObservableCollection<IBitOptionEditorViewModel>();

            foreach (IBitOption resultBitOption in this._model.ResultBitOptions)
            {
                this.BitOptionEditorViewModels.Add(this._bitOptionFactory.CreateViewModelByModelType(resultBitOption));
            }

            for (int i = 0; i < this.NumberOfBitsOnEachVariable; i++)
            {
                IAssignedBitEditorViewModel assignedBitEditorViewModel = this._assignedBitViewModelFactory.CreateViewModelByType();
                assignedBitEditorViewModel.NumberOfBit = i;
                IBitOptionEditorViewModel relatedBitOption = this.BitOptionEditorViewModels.FirstOrDefault(model => model.NumbersOfAssotiatedBits.Contains(i));
                if (relatedBitOption != null)
                {
                    assignedBitEditorViewModel.SelectedBitOptionEditorViewModel = relatedBitOption;
                }
                assignedBitEditorViewModel.BitOptionEditorViewModels = this.BitOptionEditorViewModels;
                this.AssignedBitEditorViewModels.Add(assignedBitEditorViewModel);
            }
            RaisePropertyChanged(nameof(GroupedAssignedBitEditorViewModels));
        }


        private void OnCancelExecute(object obj)
        {
            this.Model = this._unchangedMatrixTemplate;
            (obj as Window)?.Close();
        }

        private void OnSubmitExecute(object obj)
        {
            this.SaveChanges();
            (obj as Window)?.Close();
        }

        private void OnDeleteSignatureExecute(object obj)
        {
            if (obj is IVariableSignatureEditorViewModel)
            {
                this.VariableSignatureEditorViewModels.Remove(obj as IVariableSignatureEditorViewModel);
            }
        }

        private void OnDeleteMatrixVariableExecute(object obj)
        {
            if (obj is IMatrixMemoryVariableEditorViewModel)
            {
                this.MatrixMemoryVariableEditorViewModels.Remove(obj as IMatrixMemoryVariableEditorViewModel);
            }
        }

        private void OnAddSignatureExucute()
        {
            this.VariableSignatureEditorViewModels.Add(this._variableSignatureEditorViewModelFactory.CreateVariableSignatureEditorViewModel());
        }

        private void OnAddSignatureGroupExecute()
        {
            try
            {
                int j = 1;
                if (IsInversion)
                {
                    for (int i = 0; i < ValueSignatureNumberOfPoints; i++)
                    {
                        var variableModel = this._variableSignatureEditorViewModelFactory.CreateVariableSignatureEditorViewModel();
                        variableModel.Signature = ValueSignatureMask + j;
                        variableModel.IsMultipleAssgnmentAllowed = false;
                        this.VariableSignatureEditorViewModels.Add(variableModel);
                        variableModel = this._variableSignatureEditorViewModelFactory.CreateVariableSignatureEditorViewModel();
                        variableModel.Signature = ValueSignatureMask + j + _inversionString;
                        this.VariableSignatureEditorViewModels.Add(variableModel);
                        j++;
                    }
                }
                else
                {
                    for (int i = 0; i <  ValueSignatureNumberOfPoints; i++)
                    {
                        var variableModel = this._variableSignatureEditorViewModelFactory.CreateVariableSignatureEditorViewModel();
                        variableModel.Signature = ValueSignatureMask + j;
                        variableModel.IsMultipleAssgnmentAllowed = false;
                        this.VariableSignatureEditorViewModels.Add(variableModel);
                        j++;
                    }
                }

            }
            catch (Exception ex) { }
            finally
            {
                ValueSignatureMask = string.Empty;
                ValueSignatureNumberOfPoints = 0;
                IsInversion = false;
            }
        }


        private void OnAddMatrixVariableExucute()
        {
            this.MatrixMemoryVariableEditorViewModels.Add(this._matrixMemoryVariableEditorViewModelFactory.CreateMatrixMemoryVariableEditorViewModel());
        }


        public string StrongName => nameof(MatrixTemplateEditorViewModel);

        public object Model
        {
            get { return this._model; }
            set
            {
                this._model = value as IMatrixTemplate;
                this._unchangedMatrixTemplate = (value as IMatrixTemplate)?.Clone() as IMatrixTemplate;
                this.NumberOfBitsOnEachVariable = this._model.NumberOfBitsOnEachVariable;
                this.MatrixMemoryVariableEditorViewModels.Clear();
                foreach (IMatrixMemoryVariable matrixMemoryVariable in this._model.MatrixMemoryVariables)
                {
                    this.MatrixMemoryVariableEditorViewModels.Add(
                        this._matrixMemoryVariableEditorViewModelFactory.CreateMatrixMemoryVariableEditorViewModel(
                            matrixMemoryVariable));
                }
                this.VariableSignatureEditorViewModels.Clear();
                foreach (IVariableColumnSignature variableSignature in this._model.VariableColumnSignatures)
                {
                    this.VariableSignatureEditorViewModels.Add(
                        this._variableSignatureEditorViewModelFactory.CreateVariableSignatureEditorViewModel(
                            variableSignature));
                }



                IMatrixVariableOptionTemplateEditorViewModel existing = this.AvailableMatrixVariableOptionTemplateEditorViewModels.FirstOrDefault((model =>
                   (model.Model as IMatrixVariableOptionTemplate).StrongName == this._model.MatrixVariableOptionTemplate.StrongName));
                existing.Model = this._model.MatrixVariableOptionTemplate;
                this.SelectedMatrixVariableOptionTemplateEditorViewModel = existing;
                this.UpdateViewModel();
            }
        }


        private void SaveChanges()
        {
            this._model.NumberOfBitsOnEachVariable = this.NumberOfBitsOnEachVariable;
            this._model.MatrixMemoryVariables.Clear();
            foreach (IMatrixMemoryVariableEditorViewModel matrixMemoryVariableEditorViewModel in this.MatrixMemoryVariableEditorViewModels)
            {
                this._model.MatrixMemoryVariables.Add(matrixMemoryVariableEditorViewModel.Model as IMatrixMemoryVariable);
            }
            this._model.VariableColumnSignatures.Clear();
            foreach (IVariableSignatureEditorViewModel variableSignatureEditorViewModel in this.VariableSignatureEditorViewModels)
            {
                this._model.VariableColumnSignatures.Add(variableSignatureEditorViewModel.Model as IVariableColumnSignature);
            }
            this._model.MatrixVariableOptionTemplate =
                this.SelectedMatrixVariableOptionTemplateEditorViewModel.Model as IMatrixVariableOptionTemplate;
            this._model.ResultBitOptions = this.BitOptionEditorViewModels.Select((model => model.Model as IBitOption)).ToList();
        }

        public int NumberOfBitsOnEachVariable
        {
            get { return this._numberOfBitsOnEachVariable; }
            set
            {
                this._numberOfBitsOnEachVariable = value;
                this.RaisePropertyChanged();
            }

        }

        public ObservableCollection<IMatrixMemoryVariableEditorViewModel> MatrixMemoryVariableEditorViewModels { get; }
        public ObservableCollection<IVariableSignatureEditorViewModel> VariableSignatureEditorViewModels { get; }

        public List<IMatrixVariableOptionTemplateEditorViewModel> AvailableMatrixVariableOptionTemplateEditorViewModels
        {
            get { return this._availableMatrixVariableOptionTemplateEditorViewModels; }
            set
            {
                this._availableMatrixVariableOptionTemplateEditorViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IBitOptionEditorViewModel> BitOptionEditorViewModels
        {
            get { return this._bitOptionEditorViewModels; }
            set
            {
                this._bitOptionEditorViewModels = value;
                this.RaisePropertyChanged();
            }
        }

        public IMatrixVariableOptionTemplateEditorViewModel SelectedMatrixVariableOptionTemplateEditorViewModel
        {
            get { return this._selectedMatrixVariableOptionTemplateEditorViewModel; }
            set
            {
                this._selectedMatrixVariableOptionTemplateEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IAssignedBitEditorViewModel> AssignedBitEditorViewModels { get; }

        public ObservableCollection<ObservableCollection<IAssignedBitEditorViewModel>> GroupedAssignedBitEditorViewModels
        {
            get
            {
                var groupsNum = Math.Ceiling((decimal)AssignedBitEditorViewModels.Count / 16);
                var groupedAssignedBitEditorViewModels = new ObservableCollection<ObservableCollection<IAssignedBitEditorViewModel>>();
                for (var i = 0; i < groupsNum; i++)
                {
                    groupedAssignedBitEditorViewModels.Add(i == groupsNum - 1 && AssignedBitEditorViewModels.Count % 16 != 0
                        ? new ObservableCollection<IAssignedBitEditorViewModel>(AssignedBitEditorViewModels.Skip(i * 16)
                            .Take(AssignedBitEditorViewModels.Count % 16))
                        : new ObservableCollection<IAssignedBitEditorViewModel>(AssignedBitEditorViewModels.Skip(i * 16)
                            .Take(16)));
                }
                return groupedAssignedBitEditorViewModels;
            }
        }

        public string ValueSignatureMask
        {
            get { return _valueSignatureMask; }
            set
            {
                _valueSignatureMask = value;
                RaisePropertyChanged();
            }

        }


        public int ValueSignatureNumberOfPoints
        {
            get { return _valueSignatureNumberOfPoints; }
            set
            {
                _valueSignatureNumberOfPoints = value;
                RaisePropertyChanged();
            }
        }
        public bool IsInversion
        {
            get { return _isInversion; }
            set
            {
                _isInversion = value;
                RaisePropertyChanged();
            }
        }

        public ICommand AssignSignalsAutomatically { get; }
        public ICommand ClearAssignedSignals { get; }
        public ICommand OnSelectionChangedCommand { get; }
        public ICommand AddMatrixVariableCommand { get; }
        public ICommand AddSignatureCommand { get; }
        public ICommand DeleteMatrixVariableCommand { get; }
        public ICommand DeleteSignatureCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public ICommand AddSignatureGroupCommand { get; }
        public ICommand ClearSignaturesCommand { get; }

        public string MatrixName
        {
            get { return this._matrixName; }
            set
            {
                this._matrixName = value;
                this.RaisePropertyChanged();
            }
        }

        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult res = (new MatrixTemplateEditorViewModelValidator(this._localizerService)).Validate(this);
            this.SetValidationErrors(res);
        }
    }
}

