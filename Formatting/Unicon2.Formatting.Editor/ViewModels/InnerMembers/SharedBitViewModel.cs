using System.Windows.Input;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels.InnerMembers
{
    public class SharedBitViewModel : ViewModelBase, ISharedBitViewModel
    {
        private int _numberOfBit;

        public SharedBitViewModel()
        {
            ChangeValueByOwnerCommand = new RelayCommand<object>(OnChangeValueByOwnerExecute);
        }


        private void OnChangeValueByOwnerExecute(object initiator)
        {
            if (Value)
            {
                if (Owner == initiator)
                    Value = false;
                Owner = null;
            }
            else
            {
                Value = true;
                Owner = initiator;
            }
            RaisePropertyChanged(nameof(Value));
            RaisePropertyChanged(nameof(Owner));
        }

        public int NumberOfBit
        {
            get { return _numberOfBit; }
            set
            {
                _numberOfBit = value;
                RaisePropertyChanged();
            }
        }

        public bool Value { get; private set; }

        public object Owner { get; private set; }

        public ICommand ChangeValueByOwnerCommand { get; set; }
        public void Refresh()
        {
            Value = false;
            Owner = null;
            RaisePropertyChanged(nameof(Value));
            RaisePropertyChanged(nameof(Owner));
        }
    }
}
