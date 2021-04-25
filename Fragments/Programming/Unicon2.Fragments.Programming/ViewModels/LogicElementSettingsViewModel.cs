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
            window?.Close();
        }

        private void OnCloseCommand(Window window)
        {
            this._sourceViewModel.Model = _previouseViewModel.Model;
            this._sourceViewModel.Caption = _previouseViewModel.Caption;

            //for (int i = 0; i < _previouseViewModel.ConnectorViewModels.Count; i++)
            //{
            //    var connectorSource = _previouseViewModel.ConnectorViewModels[i];
            //    if (connectorSource.Connected)
            //    {
            //        var connection = connectorSource.Connection;
            //        var connectorEdited = _sourceViewModel.ConnectorViewModels[i];

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
    }
}
