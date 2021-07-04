using Unicon2.Unity.ViewModels;

namespace Unicon2.Presentation.Infrastructure.ViewModels
{
    public class BitViewModel : ViewModelBase, IBitViewModel
    {
        private bool _isChecked;
        private bool _isBitEditEnabled;


        public BitViewModel(int bitNumber, bool isBitEditEnabled, string ownerTooltip=null)
        {
            BitNumber = bitNumber;
            _isBitEditEnabled = isBitEditEnabled;
            OwnerTooltip = ownerTooltip;
        }
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                _isChecked = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsBitEditEnabled));
            }
        }

        public int BitNumber { get; }
        public bool IsBitEditEnabled => IsChecked || _isBitEditEnabled;
        public string OwnerTooltip { get; }
    }
}