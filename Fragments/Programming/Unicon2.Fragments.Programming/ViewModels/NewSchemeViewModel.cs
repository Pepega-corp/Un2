using System.Windows;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    public class NewSchemeViewModel : ViewModelBase
    {
        private string DEFAULT_NAME = "New sceme";
        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }
        public MessageDialogResult DialogResult { get; private set; }

        private readonly Size[] _sizes;
        private int _selectedIndex;
        private string _schemeName;

        public string[] SheetFormats { get; }

        public int SelectedIndex
        {
            get => this._selectedIndex;
            set
            {
                this._selectedIndex = value;
                RaisePropertyChanged();
            }
        }

        public string SchemeName
        {
            get => string.IsNullOrEmpty(this._schemeName) ? this.DEFAULT_NAME : this._schemeName;
            set
            {
                this._schemeName = value;
                RaisePropertyChanged();
            }
        }

        public Size SelectedSize => this._sizes[this.SelectedIndex];

        public NewSchemeViewModel()
        {
            this._sizes = new[]
            {
                new Size(297, 210), new Size(210, 297), new Size(420, 297), new Size(297, 420), new Size(594, 420),
                new Size(420, 594), new Size(841, 594), new Size(594, 841), new Size(1189, 841), new Size(841, 1189)
            };

            this.SheetFormats = new[]
            {
                "А4 - Портрет", "А4 - Альбом", "А3 - Портрет",  "А3 - Альбом", "А2 - Портрет",
                "А2 - Альбом", "А1 - Портрет",  "А1 - Альбом", "А0 - Портрет", "А0 - Альбом"
            };

            this.OkCommand = new RelayCommand<Window>(this.OnOkCommand);
            this.CancelCommand = new RelayCommand<Window>(this.OnCancelCommand);
        }

        private void OnOkCommand(Window window)
        {
            this.DialogResult = MessageDialogResult.Affirmative;
            window?.Close();
        }

        private void OnCancelCommand(Window window)
        {
            this.DialogResult = MessageDialogResult.Canceled;
            window?.Close();
        }
    }
}
