using System.Windows;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.Programming.ViewModels
{
    internal class LogicElementSettingsViewModel : ViewModelBase
    {
        private ILogicElementViewModel _sourceViewModel;
        private readonly ILogicElementViewModel _previouseViewModel;

        public ICommand OkCommand { get; }
        public ICommand CancelCommand { get; }

        public LogicElementSettingsViewModel(ILogicElementViewModel sourceViewModel)
        {
            this.LogicElementViewModel = sourceViewModel;
            _previouseViewModel = this._sourceViewModel.Clone();

            this.OkCommand = new RelayCommand<Window>(this.OnOkCommand);
            this.CancelCommand = new RelayCommand<Window>(this.OnCloseCommand);
        }

        public ILogicElementViewModel LogicElementViewModel
        {
            get { return this._sourceViewModel; }
            set
            {
                if (this._sourceViewModel == value) return;

                this._sourceViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        private void OnOkCommand(Window window)
        {
            if (_sourceViewModel is ISettingsApplicable settingsApplicable)
            {
                settingsApplicable.ApplySettings();
            }
            window?.Close();
        }

        private void OnCloseCommand(Window window)
        {
            this._sourceViewModel.ResetSettingsTo(_previouseViewModel.Model);
            this._sourceViewModel.Caption = _previouseViewModel.Caption;
            window?.Close();
        }
    }
}
