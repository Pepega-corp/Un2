using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Infrastructure.StructItemsInterfaces.Matrix.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates
{
    public class OptionPossibleValueEditorViewModel : ViewModelBase, IOptionPossibleValueEditorViewModel
    {
        private readonly IGeneralViewModelFactory<IPossibleValueConditionEditorViewModel> _generalViewModelFactory;
        private IOptionPossibleValue _model;
        private string _possibleValueName;
        private IPossibleValueConditionEditorViewModel _possibleValueConditionEditorViewModel;

        public OptionPossibleValueEditorViewModel(IGeneralViewModelFactory<IPossibleValueConditionEditorViewModel> generalViewModelFactory)
        {
            this._generalViewModelFactory = generalViewModelFactory;
            this.PossibleValueConditionEditorViewModels = new ObservableCollection<IPossibleValueConditionEditorViewModel>();
            this.AddConditionCommand = new RelayCommand(this.OnAddConditionExecute);
            this.ResetConditionsCommand = new RelayCommand(this.OnResetConditionsExecute);
        }

        private void OnAddConditionExecute()
        {
            this.PossibleValueConditionEditorViewModels.Add(this._generalViewModelFactory.CreateViewModelWithModelByModelType(typeof(IPossibleValueCondition)));
        }

        private void OnResetConditionsExecute()
        {
            this.PossibleValueConditionEditorViewModels.Clear();
        }


        public string StrongName => nameof(OptionPossibleValueEditorViewModel);

        public object Model
        {
            get
            {
                this._model.PossibleValueConditions.Clear();
                foreach (IPossibleValueConditionEditorViewModel possibleValueConditionEditorViewModel in this.PossibleValueConditionEditorViewModels)
                {
                    this._model.PossibleValueConditions.Add(possibleValueConditionEditorViewModel.Model as IPossibleValueCondition);
                }
                this._model.PossibleValueName = this.PossibleValueName;
                return this._model;

            }
            set
            {
                this._model = value as IOptionPossibleValue;
                this.PossibleValueConditionEditorViewModels.Clear();
                foreach (IPossibleValueCondition possibleValueCondition in this._model.PossibleValueConditions)
                {
                    this.PossibleValueConditionEditorViewModels.Add(this._generalViewModelFactory.CreateViewModelByModelType(possibleValueCondition));
                }
                this.PossibleValueName = this._model.PossibleValueName;
            }
        }

        public string PossibleValueName
        {
            get { return this._possibleValueName; }
            set
            {
                this._possibleValueName = value;
                this.RaisePropertyChanged();
            }
        }

        public ObservableCollection<IPossibleValueConditionEditorViewModel> PossibleValueConditionEditorViewModels
        {
            get;
        }

        public ICommand AddConditionCommand { get; }
        public ICommand ResetConditionsCommand { get; }
        public void SetAvailableOptionPossibleValues(List<IOptionPossibleValueEditorViewModel> optionPossibleValueEditorViewModels)
        {
            foreach (IPossibleValueConditionEditorViewModel possibleValueConditionEditorViewModel in this.PossibleValueConditionEditorViewModels)
            {
                possibleValueConditionEditorViewModel.SetAvailableOptionPossibleValueEditorViewModel(optionPossibleValueEditorViewModels);
            }
        }
    }
}
