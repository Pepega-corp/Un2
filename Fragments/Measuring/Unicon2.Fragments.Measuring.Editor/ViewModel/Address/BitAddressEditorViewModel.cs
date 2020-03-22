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
        private bool _isBitNumberInWordActual;


        public int FunctionNumber
        {
            get { return _functionNumber; }
            set
            {
                _functionNumber = value;
                if ((_functionNumber == 3) || (_functionNumber == 4))
                {
                    IsBitNumberInWordActual = true;

                }
                else
                {
                    IsBitNumberInWordActual = false;
                }

                RaisePropertyChanged();
            }
        }

        public ushort Address
        {
            get { return _address; }
            set
            {
                _address = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBitNumberInWordActual
        {
            get { return _isBitNumberInWordActual; }
            set
            {
                _isBitNumberInWordActual = value;
                RaisePropertyChanged();
            }
        }

        public ushort BitNumberInWord
        {
            get { return _bitNumberInWord; }
            set
            {
                _bitNumberInWord = value;
                RaisePropertyChanged();
            }
        }

        public string StrongName => MeasuringKeys.BIT_ADDRESS +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
		
    }
}
