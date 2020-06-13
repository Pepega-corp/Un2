using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    internal class LogicElementSettingsViewModel : ViewModelBase
    {
        private readonly ILogicElementViewModel _sourceViewModel;
        private ILogicElementViewModel _editableViewModel;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public LogicElementSettingsViewModel(ILogicElementViewModel sourceViewModel)
        {
            this._sourceViewModel = sourceViewModel;
            this.LogicElementViewModel = (ILogicElementViewModel)this._sourceViewModel.Clone();

            this.OkCommand = new RelayCommand<Window>(this.OnOkCommand);
            this.CancelCommand = new RelayCommand<Window>(this.OnCloseCommand);
        }

        public ILogicElementViewModel LogicElementViewModel
        {
            get { return this._editableViewModel; }
            set
            {
                if (this._editableViewModel == value) return;

                this._editableViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        private void OnOkCommand(Window window)
        {
            this._sourceViewModel.Model.CopyValues(this._editableViewModel.Model);

            window?.Close();
        }

        private void OnCloseCommand(Window window)
        {
            window?.Close();
        }
    }
}
