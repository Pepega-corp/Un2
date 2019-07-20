using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.Editor.ViewModel
{
    class EditElementViewModel : ViewModelBase
    {
        private readonly ILogicElementEditorViewModel _sourceViewModel;
        private ILogicElementEditorViewModel _editableViewModel;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public EditElementViewModel(ILogicElementEditorViewModel sourceViewModel)
        {
            this._sourceViewModel = sourceViewModel;
            this.EditableEditorViewModel = (ILogicElementEditorViewModel)this._sourceViewModel.Clone();

            this.OkCommand = new RelayCommand<Window>(this.OnOkCommand);
            this.CancelCommand = new RelayCommand<Window>(this.OnCloseCommand);
        }

        public ILogicElementEditorViewModel EditableEditorViewModel
        {
            get { return this._editableViewModel; }
            set
            {
                if (this._editableViewModel == value) return;

                this._editableViewModel = value;
                RaisePropertyChanged();
            }
        }

        private void OnOkCommand(Window window)
        {
            this._sourceViewModel.Model = this._editableViewModel.Model;

            window?.Close();
        }

        private void OnCloseCommand(Window window)
        {
            window?.Close();
        }
    }
}
