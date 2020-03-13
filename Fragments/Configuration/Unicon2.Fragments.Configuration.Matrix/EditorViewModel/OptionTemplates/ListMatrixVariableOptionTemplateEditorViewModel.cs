using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Keys;
using Unicon2.Fragments.Configuration.Matrix.Model.OptionTemplates;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates
{
    public class ListMatrixVariableOptionTemplateEditorViewModel : ViewModelBase, IMatrixVariableOptionTemplateEditorViewModel
    {
        private readonly IGeneralViewModelFactory<IOptionPossibleValueEditorViewModel> _generalViewModelFactory;
        private ListMatrixVariableOptionTemplate _model;
        public ICommand DeletePossibleValueCommand { get; }
        public ICommand AddNewOptionPossibleValueCommand { get; }

        public ListMatrixVariableOptionTemplateEditorViewModel(IGeneralViewModelFactory<IOptionPossibleValueEditorViewModel> generalViewModelFactory)
        {
            _generalViewModelFactory = generalViewModelFactory;
            OptionPossibleValueEditorViewModels = new ObservableCollection<IOptionPossibleValueEditorViewModel>();
            AddNewOptionPossibleValueCommand = new RelayCommand(OnAddNewOptionPossibleValueExecute);
            DeletePossibleValueCommand = new RelayCommand<object>(OnDeletePossibleValueExecute);
        }

        private void OnDeletePossibleValueExecute(object obj)
        {
            if (obj is IOptionPossibleValueEditorViewModel)
            {
                OptionPossibleValueEditorViewModels.Remove(obj as IOptionPossibleValueEditorViewModel);
            }
        }
        
        private void OnAddNewOptionPossibleValueExecute()
        {
            OptionPossibleValueEditorViewModels.Add(_generalViewModelFactory.CreateViewModelWithModelByModelType(typeof(IOptionPossibleValue)));
        }


        public string StrongName => MatrixKeys.LIST_MATRIX_TEMPLATE + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model
        {
            get
            {
                _model.OptionPossibleValues.Clear();
                foreach (IOptionPossibleValueEditorViewModel optionPossibleValue in OptionPossibleValueEditorViewModels)
                {
                    _model.OptionPossibleValues.Add(optionPossibleValue.Model as IOptionPossibleValue);
                }
                return _model;
            }
            set
            {
                _model = value as ListMatrixVariableOptionTemplate;
                OptionPossibleValueEditorViewModels.Clear();
                foreach (IOptionPossibleValue optionPossibleValue in _model.OptionPossibleValues)
                {
                    OptionPossibleValueEditorViewModels.Add(_generalViewModelFactory.CreateViewModelByModelType(optionPossibleValue));
                }
                OptionPossibleValueEditorViewModels.ForEach((model => model.SetAvailableOptionPossibleValues(OptionPossibleValueEditorViewModels.ToList())));
            }
        }


        public ObservableCollection<IOptionPossibleValueEditorViewModel> OptionPossibleValueEditorViewModels { get; }
    }
}
