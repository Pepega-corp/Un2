using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Infrastructure.Values.Matrix;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Configuration.Matrix.EditorViewModel
{
    public class MatrixMemoryVariableEditorViewModel : ViewModelBase, IMatrixMemoryVariableEditorViewModel
    {
        private IMatrixMemoryVariable _model;
        private string _name;
        private ushort _startAddress;
        private ushort _startAddressWord;
        private ushort _startAddressBit;

        public string StrongName => nameof(MatrixMemoryVariableEditorViewModel);

        public object Model
        {
            get
            {
                _model.Name = Name;
                _model.StartAddressBit = StartAddressBit;
                _model.StartAddressWord = StartAddressWord;
                return _model;

            }
            set
            {
                _model = value as IMatrixMemoryVariable;
                Name = _model.Name;
                StartAddressBit = _model.StartAddressBit;
                StartAddressWord = _model.StartAddressWord;
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                RaisePropertyChanged();
            }
        }

        public ushort StartAddress
        {
            get { return _startAddress; }
            set
            {
                _startAddress = value;
                RaisePropertyChanged();
            }
        }

        public ushort StartAddressWord
        {
            get { return _startAddressWord; }
            set
            {
                _startAddressWord = value;
                RaisePropertyChanged();
            }
        }

        public ushort StartAddressBit
        {
            get { return _startAddressBit; }
            set
            {
                _startAddressBit = value;
                RaisePropertyChanged();
            }
        }
    }
}
