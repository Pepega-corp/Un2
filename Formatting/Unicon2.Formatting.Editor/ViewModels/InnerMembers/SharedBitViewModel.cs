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
            this.ChangeValueByOwnerCommand = new RelayCommand<object>(this.OnChangeValueByOwnerExecute);
        }


        private void OnChangeValueByOwnerExecute(object initiator)
        {
            if (this.Value)
            {
                if (this.Owner == initiator)
                    this.Value = false;
                this.Owner = null;
            }
            else
            {
                this.Value = true;
                this.Owner = initiator;
            }
            this.RaisePropertyChanged(nameof(this.Value));
            this.RaisePropertyChanged(nameof(this.Owner));
        }

        public int NumberOfBit
        {
            get { return this._numberOfBit; }
            set
            {
                this._numberOfBit = value;
                this.RaisePropertyChanged();
            }
        }

        public bool Value { get; private set; }

        public object Owner { get; private set; }

        public ICommand ChangeValueByOwnerCommand { get; set; }
        public void Refresh()
        {
            this.Value = false;
            this.Owner = null;
            this.RaisePropertyChanged(nameof(this.Value));
            this.RaisePropertyChanged(nameof(this.Owner));
        }
    }
}
