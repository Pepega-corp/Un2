using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates
{
    public class ListMatrixVariableOptionTemplateEditorViewModel : ViewModelBase, IMatrixVariableOptionTemplateEditorViewModel
    {
        private readonly IGeneralViewModelFactory<IOptionPossibleValueEditorViewModel> _generalViewModelFactory;
        private ListMatrixVariableOptionTemplate _model;

        public ListMatrixVariableOptionTemplateEditorViewModel(IGeneralViewModelFactory<IOptionPossibleValueEditorViewModel> generalViewModelFactory)
        {
            this._generalViewModelFactory = generalViewModelFactory;
            this.OptionPossibleValueEditorViewModels = new ObservableCollection<IOptionPossibleValueEditorViewModel>();
            this.AddNewOptionPossibleValueCommand = new RelayCommand(this.OnAddNewOptionPossibleValueExecute);
            this.DeletePossibleValueCommand = new RelayCommand<object>(this.OnDeletePossibleValueExecute);
        }

        private void OnDeletePossibleValueExecute(object obj)
        {
            if (obj is IOptionPossibleValueEditorViewModel)
            {
                this.OptionPossibleValueEditorViewModels.Remove(obj as IOptionPossibleValueEditorViewModel);
            }
        }
        
        private void OnAddNewOptionPossibleValueExecute()
        {
            this.OptionPossibleValueEditorViewModels.Add(this._generalViewModelFactory.CreateViewModelWithModelByModelType(typeof(IOptionPossibleValue)));
        }

        public ICommand AddNewOptionPossibleValueCommand { get; }

        #region Implementation of IStronglyNamed

        public string StrongName => MatrixKeys.LIST_MATRIX_TEMPLATE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {
                this._model.OptionPossibleValues.Clear();
                foreach (IOptionPossibleValueEditorViewModel optionPossibleValue in this.OptionPossibleValueEditorViewModels)
                {
                    this._model.OptionPossibleValues.Add(optionPossibleValue.Model as IOptionPossibleValue);
                }
                return this._model;
            }
            set
            {
                this._model = value as ListMatrixVariableOptionTemplate;
                this.OptionPossibleValueEditorViewModels.Clear();
                foreach (IOptionPossibleValue optionPossibleValue in this._model.OptionPossibleValues)
                {
                    this.OptionPossibleValueEditorViewModels.Add(this._generalViewModelFactory.CreateViewModelByModelType(optionPossibleValue));
                }
                this.OptionPossibleValueEditorViewModels.ForEach((model => model.SetAvailableOptionPossibleValues(this.OptionPossibleValueEditorViewModels.ToList())));
            }
        }

        private ICommand DeletePossibleValueCommand { get; }

        public ObservableCollection<IOptionPossibleValueEditorViewModel> OptionPossibleValueEditorViewModels { get; }

        #endregion
    }
}
