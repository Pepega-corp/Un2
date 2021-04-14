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
            this.LogicElementViewModel = this._sourceViewModel.Clone();

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
            this._sourceViewModel.CopyValues(this._editableViewModel);

            //for (int i = 0; i < _sourceViewModel.ConnectorViewModels.Count; i++)
            //{
            //    var connectorSource = _sourceViewModel.ConnectorViewModels[i];
            //    if (connectorSource.Connected)
            //    {
            //        var connection = connectorSource.Connection;
            //        var connectorEdited = _editableViewModel.ConnectorViewModels[i];
                
            //        if (connection.SourceConnector == connectorSource)
            //        {
            //            connection.SourceConnector = connectorEdited;
            //        }
            //        else
            //        {
            //            connection.SinkConnectors.Remove(connectorSource);
            //            connection.SinkConnectors.Add(connectorEdited);
            //        }
            //    }
            //}

            window?.Close();
        }

        private void OnCloseCommand(Window window)
        {
            window?.Close();
        }
    }
}
