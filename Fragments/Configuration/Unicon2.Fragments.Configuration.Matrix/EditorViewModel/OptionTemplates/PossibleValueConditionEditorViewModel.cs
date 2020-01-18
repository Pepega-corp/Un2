using System.Collections.Generic;
using System.Linq;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel.OptionTemplates;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model.OptionTemplates;
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
                this._model.BoolConditionRule = this.BoolConditionRule;
                this._model.RelatedOptionPossibleValue =
                    this.RelatedOptionPossibleValueEditorViewModel.Model as IOptionPossibleValue;
                return this._model;
            }
            set
            {
                this._model = value as IPossibleValueCondition;
                this.BoolConditionRule = this._model.BoolConditionRule;
            }
        }

        public bool BoolConditionRule
        {
            get { return this._boolConditionRule; }
            set
            {
                this._boolConditionRule = value;
                this.RaisePropertyChanged();
            }
        }

        public IOptionPossibleValueEditorViewModel RelatedOptionPossibleValueEditorViewModel
        {
            get { return this._relatedOptionPossibleValueEditorViewModel; }
            set
            {
                this._relatedOptionPossibleValueEditorViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public void SetAvailableOptionPossibleValueEditorViewModel(List<IOptionPossibleValueEditorViewModel> availableOptionPossibleValueEditorViewModels)
        {
            this.RelatedOptionPossibleValueEditorViewModel =
                availableOptionPossibleValueEditorViewModels.First((model =>
                    model.Model.Equals(this._model.RelatedOptionPossibleValue)));
        }
    }
}
