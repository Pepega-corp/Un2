using System.Collections.Generic;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class BitOptionEditorViewModel : ViewModelBase, IBitOptionEditorViewModel
    {
        private IBitOption _model;
        private List<int> _numbersOfAssotiatedBits;

        public string FullSugnature { get; private set; }

        public List<int> NumbersOfAssotiatedBits
        {
            get { return _numbersOfAssotiatedBits; }
            set
            {
                _numbersOfAssotiatedBits = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsEnabled));
            }
        }

        public bool IsEnabled => !(NumbersOfAssotiatedBits.Count > 0 && !_model.VariableColumnSignature.IsMultipleAssignmentAllowed);
        public void UpdateIsEnabled()
        {
            RaisePropertyChanged(nameof(IsEnabled));
        }

        public string StrongName => nameof(BitOptionEditorViewModel);

        public object Model
        {
            get
            {
                _model.NumbersOfAssotiatedBits = NumbersOfAssotiatedBits;
                return _model;
            }
            set
            {

                _model = value as IBitOption;
                FullSugnature = _model.FullSignature;
                NumbersOfAssotiatedBits = _model.NumbersOfAssotiatedBits;
                RaisePropertyChanged(nameof(FullSugnature));
            }
        }

        public override string ToString()
        {
            return FullSugnature;
        }
    }
}
