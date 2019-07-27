using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels
{
    public class EditableBase : ViewModelBase
    {
        private string _baseString;
        private bool _isEditing;

        public EditableBase(int newIndex)
        {
            this.Value = "Base" + newIndex;
        }

        public EditableBase(string @base)
        {
            this.Value = @base;
        }

        public string Value
        {
            get => this._baseString;
            set
            {
                this._baseString = value;
                RaisePropertyChanged();
            }
        }

        public bool IsEditing
        {
            get => this._isEditing;
            set
            {
                this._isEditing = value;
                RaisePropertyChanged();
            }
        }
    }
}
