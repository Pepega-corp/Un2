using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Infrastructure.Values.Matrix.OptionTemplates;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel.OptionTemplates
{
    public class PossibleValueConditionEditorViewModel : ViewModelBase, IPossibleValueConditionEditorViewModel
    {
        private IPossibleValueCondition _model;
        private bool _boolConditionRule;
        private IOptionPossibleValueEditorViewModel _relatedOptionPossibleValueEditorViewModel;

        public string StrongName => nameof(PossibleValueConditionEditorViewModel);

        public object Model
        {
            get
            {
                _model.BoolConditionRule = BoolConditionRule;
                _model.RelatedOptionPossibleValue =
                    RelatedOptionPossibleValueEditorViewModel.Model as IOptionPossibleValue;
                return _model;
            }
            set
            {
                _model = value as IPossibleValueCondition;
                BoolConditionRule = _model.BoolConditionRule;
            }
        }

        public bool BoolConditionRule
        {
            get { return _boolConditionRule; }
            set
            {
                _boolConditionRule = value;
                RaisePropertyChanged();
            }
        }

        public IOptionPossibleValueEditorViewModel RelatedOptionPossibleValueEditorViewModel
        {
            get { return _relatedOptionPossibleValueEditorViewModel; }
            set
            {
                _relatedOptionPossibleValueEditorViewModel = value;
                RaisePropertyChanged();
            }
        }

        public void SetAvailableOptionPossibleValueEditorViewModel(List<IOptionPossibleValueEditorViewModel> availableOptionPossibleValueEditorViewModels)
        {
            RelatedOptionPossibleValueEditorViewModel =
                availableOptionPossibleValueEditorViewModels.First((model =>
                    model.Model.Equals(_model.RelatedOptionPossibleValue)));
        }
    }
}
