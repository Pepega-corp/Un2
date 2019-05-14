using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class BitOptionEditorViewModel : ViewModelBase, IBitOptionEditorViewModel
    {
        private IBitOption _model;
        private List<int> _numbersOfAssotiatedBits;

        #region Implementation of IBitOptionEditorViewModel

        public string FullSugnature { get; private set; }

        public List<int> NumbersOfAssotiatedBits
        {
            get { return this._numbersOfAssotiatedBits; }
            set
            {
                this._numbersOfAssotiatedBits = value;
                this.RaisePropertyChanged();
                this.RaisePropertyChanged(nameof(this.IsEnabled));
            }
        }

        public bool IsEnabled => !(this.NumbersOfAssotiatedBits.Count > 0 && !this._model.VariableSignature.IsMultipleAssgnmentAllowed);
        public void UpdateIsEnabled()
        {
            this.RaisePropertyChanged(nameof(this.IsEnabled));
        }

        #endregion

        #region Implementation of IStronglyNamed

        public string StrongName => nameof(BitOptionEditorViewModel);

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {
                this._model.NumbersOfAssotiatedBits = this.NumbersOfAssotiatedBits;
                return this._model;
            }
            set
            {

                this._model = value as IBitOption;
                this.FullSugnature = this._model.FullSignature;
                this.NumbersOfAssotiatedBits = this._model.NumbersOfAssotiatedBits;
                this.RaisePropertyChanged(nameof(this.FullSugnature));
            }
        }

        #endregion
    }
}
