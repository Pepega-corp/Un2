using Unicon2.Fragments.Configuration.Matrix.Interfaces.EditorViewModel;
using Unicon2.Fragments.Configuration.Matrix.Interfaces.Model;
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

        #region Implementation of IStronglyNamed

        public string StrongName => nameof(MatrixMemoryVariableEditorViewModel);

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {
                this._model.Name = this.Name;
                this._model.StartAddressBit = this.StartAddressBit;
                this._model.StartAddressWord = this.StartAddressWord;
                return this._model;

            }
            set
            {
                this._model = value as IMatrixMemoryVariable;
                this.Name = this._model.Name;
                this.StartAddressBit = this._model.StartAddressBit;
                this.StartAddressWord = this._model.StartAddressWord;
            }
        }

        #endregion

        #region Implementation of IMatrixMemoryVariableEditorViewModel

        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort StartAddress
        {
            get { return this._startAddress; }
            set
            {
                this._startAddress = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort StartAddressWord
        {
            get { return this._startAddressWord; }
            set
            {
                this._startAddressWord = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort StartAddressBit
        {
            get { return this._startAddressBit; }
            set
            {
                this._startAddressBit = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion
    }
}
