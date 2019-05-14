using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.ModbusMemory.Model
{
    public class MemoryBitViewModel : ViewModelBase, IMemoryBitViewModel
    {
        private bool? _boolValue;
        private int _bitNumber;
        private bool _isError;

        public bool IsError
        {
            get { return this._isError; }
            set
            {
                if (this._isError == value) return;
                this._isError = value;
                this.RaisePropertyChanged();
            }
        }

        public bool? BoolValue
        {
            get { return this._boolValue; }
            set
            {
                if (this._boolValue == value) return;
                this._boolValue = value;
                this.RaisePropertyChanged();
            }
        }

        public int BitNumber
        {
            get { return this._bitNumber; }
            set
            {
                if (this._bitNumber == value) return;
                this._bitNumber = value;
                this.RaisePropertyChanged();
            }
        }


        public string AddressAsHex { get; private set; }

        public string AddressAsDec { get; private set; }

        public ushort Address { get; private set; }

        public void SetAddressDec(ushort address)
        {
            this.Address = address;

            this.AddressAsDec = this.Address.ToString("D");
            this.AddressAsHex = this.Address.ToString("X");

            this.RaisePropertyChanged(nameof(this.AddressAsDec));
            this.RaisePropertyChanged(nameof(this.AddressAsHex));
        }
    }
}
