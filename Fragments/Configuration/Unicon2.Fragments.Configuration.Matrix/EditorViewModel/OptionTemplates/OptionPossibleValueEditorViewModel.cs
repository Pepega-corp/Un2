using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Infrastructure.GeneralFactories;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates
{
    public class OptionPossibleValueEditorViewModel : ViewModelBase, IOptionPossibleValueEditorViewModel
    {
        private readonly IGeneralViewModelFactory<IPossibleValueConditionEditorViewModel> _generalViewModelFactory;
        private IOptionPossibleValue _model;
        private string _possibleValueName;

        public OptionPossibleValueEditorViewModel(IGeneralViewModelFactory<IPossibleValueConditionEditorViewModel> generalViewModelFactory)
        {
            _generalViewModelFactory = generalViewModelFactory;
            PossibleValueConditionEditorViewModels = new ObservableCollection<IPossibleValueConditionEditorViewModel>();
            AddConditionCommand = new RelayCommand(OnAddConditionExecute);
            ResetConditionsCommand = new RelayCommand(OnResetConditionsExecute);
        }

        private void OnAddConditionExecute()
        {
            PossibleValueConditionEditorViewModels.Add(_generalViewModelFactory.CreateViewModelWithModelByModelType(typeof(IPossibleValueCondition)));
        }

        private void OnResetConditionsExecute()
        {
            PossibleValueConditionEditorViewModels.Clear();
        }


        public string StrongName => nameof(OptionPossibleValueEditorViewModel);

        public object Model
        {
            get
            {
                _model.PossibleValueConditions.Clear();
                foreach (IPossibleValueConditionEditorViewModel possibleValueConditionEditorViewModel in PossibleValueConditionEditorViewModels)
                {
                    _model.PossibleValueConditions.Add(possibleValueConditionEditorViewModel.Model as IPossibleValueCondition);
                }
                _model.PossibleValueName = PossibleValueName;
                return _model;

            }
            set
            {
                _model = value as IOptionPossibleValue;
                PossibleValueConditionEditorViewModels.Clear();
                foreach (IPossibleValueCondition possibleValueCondition in _model.PossibleValueConditions)
                {
                    PossibleValueConditionEditorViewModels.Add(_generalViewModelFactory.CreateViewModelByModelType(possibleValueCondition));
                }
                PossibleValueName = _model.PossibleValueName;
            }
        }

        public string PossibleValueName
        {
            get { return _possibleValueName; }
            set
            {
                _possibleValueName = value;
                RaisePropertyChanged();
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
            foreach (IPossibleValueConditionEditorViewModel possibleValueConditionEditorViewModel in PossibleValueConditionEditorViewModels)
            {
                possibleValueConditionEditorViewModel.SetAvailableOptionPossibleValueEditorViewModel(optionPossibleValueEditorViewModels);
            }
        }
    }
}
