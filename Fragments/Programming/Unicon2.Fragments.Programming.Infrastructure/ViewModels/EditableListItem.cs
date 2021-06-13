using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Infrastructure.ViewModels
{
    public class EditableListItem : ViewModelBase
    {
        private string _baseString;
        private bool _isEditing;

        public EditableListItem(string value)
        {
            this.Value = value;
        }

        public EditableListItem(EditableListItem source)
        {
            this.Value = source != null ? source.Value : "item";
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
