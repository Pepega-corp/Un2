using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.Factories;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class MatrixTemplateEditorViewModel : ViewModelBase, IMatrixTemplateEditorViewModel
    {
        private readonly IMatrixMemoryVariableEditorViewModelFactory _matrixMemoryVariableEditorViewModelFactory;
        private readonly IVariableSignatureEditorViewModelFactory _variableSignatureEditorViewModelFactory;
        private readonly IGeneralViewModelFactory<IMatrixVariableOptionTemplateEditorViewModel> _generalViewModelFactory;
        private readonly IMatrixVariableOptionTemplateEditorViewModelFactory _matrixVariableOptionTemplateEditorViewModelFactory;
        private readonly IGeneralViewModelFactory<IBitOptionEditorViewModel> _bitOptionFactory;
        private readonly IGeneralViewModelFactory<IAssignedBitEditorViewModel> _assignedBitViewModelFactory;
        private IMatrixTemplate _model;
        private int _numberOfBitsOnEachVariable;
        private List<IMatrixVariableOptionTemplateEditorViewModel> _availableMatrixVariableOptionTemplateEditorViewModels;
        private IMatrixVariableOptionTemplateEditorViewModel _selectedMatrixVariableOptionTemplateEditorViewModel;
        private List<IAssignedBitEditorViewModel> _assignedBitEditorViewModels;
        private ObservableCollection<IBitOptionEditorViewModel> _bitOptionEditorViewModels;
        private string _matrixName;

        public MatrixTemplateEditorViewModel(
            IMatrixMemoryVariableEditorViewModelFactory matrixMemoryVariableEditorViewModelFactory,
            IVariableSignatureEditorViewModelFactory variableSignatureEditorViewModelFactory,
            IGeneralViewModelFactory<IMatrixVariableOptionTemplateEditorViewModel> generalViewModelFactory,
            IMatrixVariableOptionTemplateEditorViewModelFactory matrixVariableOptionTemplateEditorViewModelFactory,
            IGeneralViewModelFactory<IBitOptionEditorViewModel> bitOptionFactory, IGeneralViewModelFactory<IAssignedBitEditorViewModel> assignedBitViewModelFactory)
        {
            this._matrixMemoryVariableEditorViewModelFactory = matrixMemoryVariableEditorViewModelFactory;
            this._variableSignatureEditorViewModelFactory = variableSignatureEditorViewModelFactory;
            this._generalViewModelFactory = generalViewModelFactory;
            this._matrixVariableOptionTemplateEditorViewModelFactory = matrixVariableOptionTemplateEditorViewModelFactory;
            this._bitOptionFactory = bitOptionFactory;
            this._assignedBitViewModelFactory = assignedBitViewModelFactory;
            this.MatrixMemoryVariableEditorViewModels = new ObservableCollection<IMatrixMemoryVariableEditorViewModel>();
            this.AddMatrixVariableCommand = new RelayCommand(this.OnAddMatrixVariableExucute);
            this.VariableSignatureEditorViewModels = new ObservableCollection<IVariableSignatureEditorViewModel>();
            this.AddSignatureCommand = new RelayCommand(this.OnAddSignatureExucute);
            this.DeleteMatrixVariableCommand = new RelayCommand<object>(this.OnDeleteMatrixVariableExecute);
            this.DeleteSignatureCommand = new RelayCommand<object>(this.OnDeleteSignatureExecute);
            this.SubmitCommand = new RelayCommand<object>(this.OnSubmitExecute);
            this.CancelCommand = new RelayCommand<object>(this.OnCancelExecute);
            this.AvailableMatrixVariableOptionTemplateEditorViewModels = 
                this._matrixVariableOptionTemplateEditorViewModelFactory.CreateAvailableMatrixVariableOptionTemplateEditorViewModel();
            this.OnSelectionChangedCommand = new RelayCommand<object>(this.OnUpdateViewModelExecute);
            this._bitOptionEditorViewModels = new ObservableCollection<IBitOptionEditorViewModel>();
            this.AssignedBitEditorViewModels = new ObservableCollection<IAssignedBitEditorViewModel>();
        }

        private void OnUpdateViewModelExecute(object o)
        {
            //if (o is SelectionChangedEventArgs)
            //{
            //    if (!(((SelectionChangedEventArgs) o).OriginalSource is TabControl))
            //    {
            //        return;
            //    }
            //}
            if (!(o is TabControl)) return;

            this.SaveChanges();
            this.UpdateViewModel();
        }

        private void UpdateViewModel()
        {

            this._model.UpdateResultBitOptions();
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
        }


        private void OnCancelExecute(object obj)
        {
            this.Model = this._model;
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

        private void OnAddMatrixVariableExucute()
        {
            this.MatrixMemoryVariableEditorViewModels.Add(this._matrixMemoryVariableEditorViewModelFactory.CreateMatrixMemoryVariableEditorViewModel());
        }


        #region Implementation of IStronglyNamed

        public string StrongName => nameof(MatrixTemplateEditorViewModel);

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return this._model; }
            set
            {
                this._model = value as IMatrixTemplate;
                this.NumberOfBitsOnEachVariable = this._model.NumberOfBitsOnEachVariable;
                this.MatrixMemoryVariableEditorViewModels.Clear();
                foreach (IMatrixMemoryVariable matrixMemoryVariable in this._model.MatrixMemoryVariables)
                {
                    this.MatrixMemoryVariableEditorViewModels.Add(
                        this._matrixMemoryVariableEditorViewModelFactory.CreateMatrixMemoryVariableEditorViewModel(
                            matrixMemoryVariable));
                }
                this.VariableSignatureEditorViewModels.Clear();
                foreach (IVariableSignature variableSignature in this._model.VariableOptionSignatures)
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
            this._model.VariableOptionSignatures.Clear();
            foreach (IVariableSignatureEditorViewModel variableSignatureEditorViewModel in this.VariableSignatureEditorViewModels)
            {
                this._model.VariableOptionSignatures.Add(variableSignatureEditorViewModel.Model as IVariableSignature);
            }
            this._model.MatrixVariableOptionTemplate =
                this.SelectedMatrixVariableOptionTemplateEditorViewModel.Model as IMatrixVariableOptionTemplate;
            this._model.ResultBitOptions = this.BitOptionEditorViewModels.Select((model => model.Model as IBitOption)).ToList();
        }

        #endregion

        #region Implementation of IMatrixTemplateEditorViewModel

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

        public ICommand OnSelectionChangedCommand { get; }
        public ICommand AddMatrixVariableCommand { get; }
        public ICommand AddSignatureCommand { get; }
        public ICommand DeleteMatrixVariableCommand { get; }
        public ICommand DeleteSignatureCommand { get; }
        public ICommand SubmitCommand { get; }
        public ICommand CancelCommand { get; }

        public string MatrixName
        {
            get { return this._matrixName; }
            set
            {
                this._matrixName = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion
    }
}
