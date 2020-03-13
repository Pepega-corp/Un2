using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using ControlzEx.Standard;
using MahApps.Metro.Controls.Dialogs;
using Unicon2.Fragments.Configuration.Matrix.EditorViewModel.Validators;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Infrastructure.Values.Matrix.Helpers;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{

    public class MatrixTemplateEditorViewModel : ValidatableBindableBase, IMatrixTemplateEditorViewModel
    {
        private readonly IMatrixMemoryVariableEditorViewModelFactory _matrixMemoryVariableEditorViewModelFactory;
        private readonly IVariableSignatureEditorViewModelFactory _variableSignatureEditorViewModelFactory;

        private readonly IGeneralViewModelFactory<IMatrixVariableOptionTemplateEditorViewModel>
            _generalViewModelFactory;

        private readonly IMatrixVariableOptionTemplateEditorViewModelFactory
            _matrixVariableOptionTemplateEditorViewModelFactory;

        private readonly IGeneralViewModelFactory<IBitOptionEditorViewModel> _bitOptionFactory;
        private readonly IGeneralViewModelFactory<IAssignedBitEditorViewModel> _assignedBitViewModelFactory;
        private readonly IBitOptionUpdatingStrategy _bitOptionUpdatingStrategy;
        private readonly ILocalizerService _localizerService;
        private readonly IDialogCoordinator _dialogCoordinator;
        private IMatrixTemplate _model;
        private int _numberOfBitsOnEachVariable;

        private List<IMatrixVariableOptionTemplateEditorViewModel>
            _availableMatrixVariableOptionTemplateEditorViewModels;

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
            IBitOptionUpdatingStrategy bitOptionUpdatingStrategy, ILocalizerService localizerService,
            IDialogCoordinator dialogCoordinator)
        {
            _matrixMemoryVariableEditorViewModelFactory = matrixMemoryVariableEditorViewModelFactory;
            _variableSignatureEditorViewModelFactory = variableSignatureEditorViewModelFactory;
            _generalViewModelFactory = generalViewModelFactory;
            _matrixVariableOptionTemplateEditorViewModelFactory =
                matrixVariableOptionTemplateEditorViewModelFactory;
            _bitOptionFactory = bitOptionFactory;
            _assignedBitViewModelFactory = assignedBitViewModelFactory;
            _bitOptionUpdatingStrategy = bitOptionUpdatingStrategy;
            _localizerService = localizerService;
            _dialogCoordinator = dialogCoordinator;
            MatrixMemoryVariableEditorViewModels =
                new ObservableCollection<IMatrixMemoryVariableEditorViewModel>();
            AddMatrixVariableCommand = new RelayCommand(OnAddMatrixVariableExucute);
            VariableSignatureEditorViewModels = new ObservableCollection<IVariableSignatureEditorViewModel>();
            AddSignatureCommand = new RelayCommand(OnAddSignatureExucute);
            DeleteMatrixVariableCommand = new RelayCommand<object>(OnDeleteMatrixVariableExecute);
            DeleteSignatureCommand = new RelayCommand<object>(OnDeleteSignatureExecute);
            SubmitCommand = new RelayCommand<object>(OnSubmitExecute, CanExecuteSubmit);
            CancelCommand = new RelayCommand<object>(OnCancelExecute);
            AssignSignalsAutomatically = new RelayCommand(OnAssignSignalsAutomatically);
            ClearAssignedSignals = new RelayCommand(OnClearAssignedSignals);
            ClearSignaturesCommand = new RelayCommand(OnClearSignatures);
            AddSignatureGroupCommand = new RelayCommand(OnAddSignatureGroupExecute);
            AvailableMatrixVariableOptionTemplateEditorViewModels =
                _matrixVariableOptionTemplateEditorViewModelFactory
                    .CreateAvailableMatrixVariableOptionTemplateEditorViewModel();
            _bitOptionEditorViewModels = new ObservableCollection<IBitOptionEditorViewModel>();
            AssignedBitEditorViewModels = new ObservableCollection<IAssignedBitEditorViewModel>();

            ValueSignatureMask = string.Empty;
            ValueSignatureNumberOfPoints = 0;
        }

        private void OnAssignSignalsAutomatically()
        {
            var minFromAsingableAndOptions =
                Math.Min(AssignedBitEditorViewModels.Count, _bitOptionEditorViewModels.Count);
            for (int i = 0; i < minFromAsingableAndOptions; i++)
            {
                AssignedBitEditorViewModels[i].SelectedBitOptionEditorViewModel = _bitOptionEditorViewModels[i];
            }

        }

        private void OnClearAssignedSignals()
        {
            var minFromAsingableAndOptions =
                Math.Min(AssignedBitEditorViewModels.Count, _bitOptionEditorViewModels.Count);
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
                    SaveChanges();
                    UpdateViewModel();
                }

                RaisePropertyChanged();
            }
        }

        private void UpdateViewModel()
        {
            _bitOptionUpdatingStrategy.UpdateBitOptions(_model);
            //   this._model.UpdateResultBitOptions();
            AssignedBitEditorViewModels.Clear();
            BitOptionEditorViewModels = new ObservableCollection<IBitOptionEditorViewModel>();

            foreach (IBitOption resultBitOption in _model.ResultBitOptions)
            {
                BitOptionEditorViewModels.Add(_bitOptionFactory.CreateViewModelByModelType(resultBitOption));
            }

            for (int i = 0; i < NumberOfBitsOnEachVariable; i++)
            {
                IAssignedBitEditorViewModel assignedBitEditorViewModel =
                    _assignedBitViewModelFactory.CreateViewModelByType();
                assignedBitEditorViewModel.NumberOfBit = i;
                IBitOptionEditorViewModel relatedBitOption =
                    BitOptionEditorViewModels.FirstOrDefault(model => model.NumbersOfAssotiatedBits.Contains(i));
                if (relatedBitOption != null)
                {
                    assignedBitEditorViewModel.SelectedBitOptionEditorViewModel = relatedBitOption;
                }

                assignedBitEditorViewModel.BitOptionEditorViewModels = BitOptionEditorViewModels;
                AssignedBitEditorViewModels.Add(assignedBitEditorViewModel);
            }

            RaisePropertyChanged(nameof(GroupedAssignedBitEditorViewModels));
        }


        private void OnCancelExecute(object obj)
        {
            Model = _unchangedMatrixTemplate;
            (obj as Window)?.Close();
        }

        private void OnSubmitExecute(object obj)
        {
            SaveChanges();
            (obj as Window)?.Close();
        }

        private void OnDeleteSignatureExecute(object obj)
        {
            if (obj is IVariableSignatureEditorViewModel)
            {
                VariableSignatureEditorViewModels.Remove(obj as IVariableSignatureEditorViewModel);
            }
        }

        private void OnDeleteMatrixVariableExecute(object obj)
        {
            if (obj is IMatrixMemoryVariableEditorViewModel)
            {
                MatrixMemoryVariableEditorViewModels.Remove(obj as IMatrixMemoryVariableEditorViewModel);
            }
        }

        private void OnAddSignatureExucute()
        {
            VariableSignatureEditorViewModels.Add(_variableSignatureEditorViewModelFactory
                .CreateVariableSignatureEditorViewModel());
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
                        var variableModel = _variableSignatureEditorViewModelFactory
                            .CreateVariableSignatureEditorViewModel();
                        variableModel.Signature = ValueSignatureMask + j;
                        variableModel.IsMultipleAssgnmentAllowed = false;
                        VariableSignatureEditorViewModels.Add(variableModel);
                        variableModel = _variableSignatureEditorViewModelFactory
                            .CreateVariableSignatureEditorViewModel();
                        variableModel.Signature = ValueSignatureMask + j + _inversionString;
                        VariableSignatureEditorViewModels.Add(variableModel);
                        j++;
                    }
                }
                else
                {
                    for (int i = 0; i < ValueSignatureNumberOfPoints; i++)
                    {
                        var variableModel = _variableSignatureEditorViewModelFactory
                            .CreateVariableSignatureEditorViewModel();
                        variableModel.Signature = ValueSignatureMask + j;
                        variableModel.IsMultipleAssgnmentAllowed = false;
                        VariableSignatureEditorViewModels.Add(variableModel);
                        j++;
                    }
                }

            }
            catch (Exception ex)
            {
            }
            finally
            {
                ValueSignatureMask = string.Empty;
                ValueSignatureNumberOfPoints = 0;
                IsInversion = false;
            }
        }


        private void OnAddMatrixVariableExucute()
        {
            MatrixMemoryVariableEditorViewModels.Add(_matrixMemoryVariableEditorViewModelFactory
                .CreateMatrixMemoryVariableEditorViewModel());
        }


        public string StrongName => nameof(MatrixTemplateEditorViewModel);

        public object Model
        {
            get { return _model; }
            set
            {
                _model = value as IMatrixTemplate;
                _unchangedMatrixTemplate = (value as IMatrixTemplate)?.Clone() as IMatrixTemplate;
                NumberOfBitsOnEachVariable = _model.NumberOfBitsOnEachVariable;
                MatrixMemoryVariableEditorViewModels.Clear();
                foreach (IMatrixMemoryVariable matrixMemoryVariable in _model.MatrixMemoryVariables)
                {
                    MatrixMemoryVariableEditorViewModels.Add(
                        _matrixMemoryVariableEditorViewModelFactory.CreateMatrixMemoryVariableEditorViewModel(
                            matrixMemoryVariable));
                }

                VariableSignatureEditorViewModels.Clear();
                foreach (IVariableColumnSignature variableSignature in _model.VariableColumnSignatures)
                {
                    VariableSignatureEditorViewModels.Add(
                        _variableSignatureEditorViewModelFactory.CreateVariableSignatureEditorViewModel(
                            variableSignature));
                }



                IMatrixVariableOptionTemplateEditorViewModel existing =
                    AvailableMatrixVariableOptionTemplateEditorViewModels.FirstOrDefault((model =>
                        (model.Model as IMatrixVariableOptionTemplate).StrongName ==
                        _model.MatrixVariableOptionTemplate.StrongName));
                existing.Model = _model.MatrixVariableOptionTemplate;
                SelectedMatrixVariableOptionTemplateEditorViewModel = existing;
                UpdateViewModel();
            }
        }


        private void SaveChanges()
        {
            _model.NumberOfBitsOnEachVariable = NumberOfBitsOnEachVariable;
            _model.MatrixMemoryVariables.Clear();
            foreach (IMatrixMemoryVariableEditorViewModel matrixMemoryVariableEditorViewModel in MatrixMemoryVariableEditorViewModels)
            {
                _model.MatrixMemoryVariables.Add(
                    matrixMemoryVariableEditorViewModel.Model as IMatrixMemoryVariable);
            }

            _model.VariableColumnSignatures.Clear();
            foreach (IVariableSignatureEditorViewModel variableSignatureEditorViewModel in VariableSignatureEditorViewModels)
            {
                _model.VariableColumnSignatures.Add(
                    variableSignatureEditorViewModel.Model as IVariableColumnSignature);
            }

            _model.MatrixVariableOptionTemplate =
                SelectedMatrixVariableOptionTemplateEditorViewModel.Model as IMatrixVariableOptionTemplate;
            _model.ResultBitOptions =
                BitOptionEditorViewModels.Select((model => model.Model as IBitOption)).ToList();
        }

        public int NumberOfBitsOnEachVariable
        {
            get { return _numberOfBitsOnEachVariable; }
            set
            {
                _numberOfBitsOnEachVariable = value;
                RaisePropertyChanged();
            }

        }

        public ObservableCollection<IMatrixMemoryVariableEditorViewModel> MatrixMemoryVariableEditorViewModels { get; }
        public ObservableCollection<IVariableSignatureEditorViewModel> VariableSignatureEditorViewModels { get; }

        public List<IMatrixVariableOptionTemplateEditorViewModel> AvailableMatrixVariableOptionTemplateEditorViewModels
        {
            get { return _availableMatrixVariableOptionTemplateEditorViewModels; }
            set
            {
                _availableMatrixVariableOptionTemplateEditorViewModels = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IBitOptionEditorViewModel> BitOptionEditorViewModels
        {
            get { return _bitOptionEditorViewModels; }
            set
            {
                _bitOptionEditorViewModels = value;
                RaisePropertyChanged();
            }
        }

        public IMatrixVariableOptionTemplateEditorViewModel SelectedMatrixVariableOptionTemplateEditorViewModel
        {
            get { return _selectedMatrixVariableOptionTemplateEditorViewModel; }
            set
            {
                _selectedMatrixVariableOptionTemplateEditorViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<IAssignedBitEditorViewModel> AssignedBitEditorViewModels { get; }

        public ObservableCollection<ObservableCollection<IAssignedBitEditorViewModel>>
            GroupedAssignedBitEditorViewModels
        {
            get
            {
                var groupsNum = Math.Ceiling((decimal) AssignedBitEditorViewModels.Count / 16);
                var groupedAssignedBitEditorViewModels =
                    new ObservableCollection<ObservableCollection<IAssignedBitEditorViewModel>>();
                for (var i = 0; i < groupsNum; i++)
                {
                    groupedAssignedBitEditorViewModels.Add(
                        i == groupsNum - 1 && AssignedBitEditorViewModels.Count % 16 != 0
                            ? new ObservableCollection<IAssignedBitEditorViewModel>(AssignedBitEditorViewModels
                                .Skip(i * 16)
                                .Take(AssignedBitEditorViewModels.Count % 16))
                            : new ObservableCollection<IAssignedBitEditorViewModel>(AssignedBitEditorViewModels
                                .Skip(i * 16)
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
            get { return _matrixName; }
            set
            {
                _matrixName = value;
                RaisePropertyChanged();
            }
        }

        protected override void OnValidate()
        {
            FluentValidation.Results.ValidationResult res =
                (new MatrixTemplateEditorViewModelValidator(_localizerService)).Validate(this);
            SetValidationErrors(res);
        }
    }
}