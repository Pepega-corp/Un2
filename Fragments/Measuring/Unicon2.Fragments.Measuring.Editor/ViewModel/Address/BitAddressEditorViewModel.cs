using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Infrastructure;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Measuring.Editor.ViewModel.Address
{
    public class BitAddressEditorViewModel : ViewModelBase, IBitAddressEditorViewModel
    {
        private int _functionNumber;
        private ushort _address;
        private ushort _bitNumberInWord;
        private IAddressOfBit _model;
        private bool _isBitNumberInWordActual;

        public BitAddressEditorViewModel(IAddressOfBit addressOfBit)
        {
            this._model = addressOfBit;
        }
        
        #region Implementation of IBitAddressEditorViewModel

        public int FunctionNumber
        {
            get { return this._functionNumber; }
            set
            {
                this._functionNumber = value;
                if ((this._functionNumber == 3) || (this._functionNumber == 4))
                {
                    this.IsBitNumberInWordActual = true;

                }
                else
                {
                    this.IsBitNumberInWordActual = false;
                }

                this.RaisePropertyChanged();
            }
        }

        public ushort Address
        {
            get { return this._address; }
            set
            {
                this._address = value;
                this.RaisePropertyChanged();
            }
        }

        public bool IsBitNumberInWordActual
        {
            get { return this._isBitNumberInWordActual; }
            set
            {
                this._isBitNumberInWordActual = value;
                this.RaisePropertyChanged();
            }
        }

        public ushort BitNumberInWord
        {
            get { return this._bitNumberInWord; }
            set
            {
                this._bitNumberInWord = value;
                this.RaisePropertyChanged();
            }
        }

        #endregion
        
        #region Implementation of IStronglyNamed

        public string StrongName => MeasuringKeys.BIT_ADDRESS +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get
            {
                this._model.Address = this.Address;
                this._model.BitAddressInWord = this.BitNumberInWord;
                this._model.NumberOfFunction = this.FunctionNumber;
                return this._model;

            }
            set
            {
                this._model = value as IAddressOfBit;
                this.BitNumberInWord = this._model.BitAddressInWord;
                this.Address = this._model.Address;
                this.FunctionNumber = this._model.NumberOfFunction;
            }
        }

        #endregion
    }
}
