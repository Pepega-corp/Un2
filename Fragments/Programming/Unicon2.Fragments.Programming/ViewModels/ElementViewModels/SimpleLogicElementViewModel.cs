using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class SimpleLogicElementViewModel : LogicElementViewModel, ISettingsApplicable
    {
        private List<IConnectorViewModel> _addConnectors = new List<IConnectorViewModel>();
        private List<IConnectorViewModel> _removeConnectors = new List<IConnectorViewModel>();
        
        public ICommand AddInputCommand { get; }
        public ICommand RemoveInputCommand { get; }
        public int Width => 20;
        public int Height => Inputs.Count * 10 + 10;
        public ObservableCollection<IConnectorViewModel> Inputs { get; }
        public ObservableCollection<IConnectorViewModel> Outputs { get; }

        protected SimpleLogicElementViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _logicElementModel = model;
            this._globalCommands = globalCommands;
            AddInputCommand = new RelayCommand(AddInput, CanAddInput);
            RemoveInputCommand = new RelayCommand(RemoveInput, CanRemove);
            Inputs = new ObservableCollection<IConnectorViewModel>();
            Outputs = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels.CollectionChanged += OnConnectorsCollectionChanged;
            SetModel(model);
        }
        
        private void OnConnectorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in eventArgs.NewItems.Cast<IConnectorViewModel>())
                    {
                        AddConnector(c);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var c in eventArgs.OldItems.Cast<IConnectorViewModel>())
                    {
                        RemoveConnector(c);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Inputs.Clear();
                    Outputs.Clear();
                    break;
            }

            RaisePropertyChanged(nameof(Height));
            RaisePropertyChanged(nameof(Width));
        }

        private void AddConnector(IConnectorViewModel connector)
        {
            if (connector.Orientation == ConnectorOrientation.LEFT)
            {
                Inputs.Add(connector);
            }
            else
            {
                Outputs.Add(connector);
            }
        }

        private void RemoveConnector(IConnectorViewModel connector)
        {
            if (connector.Orientation == ConnectorOrientation.LEFT)
            {
                Inputs.Remove(connector);
            }
            else
            {
                Outputs.Remove(connector);
            }
        }

        private bool CanAddInput()
        {
            return Inputs.Count < 8;
        }

        private void AddInput()
        {
            ConnectorViewModels.Add(new ConnectorViewModel(this, ConnectorOrientation.LEFT, ConnectorType.DIRECT));
            ((RelayCommand)AddInputCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RemoveInputCommand).RaiseCanExecuteChanged();
        }

        private bool CanRemove()
        {
            return Inputs.Count > 2;
        }

        private void RemoveInput()
        {
            var lastConnector = ConnectorViewModels.Last();
            lastConnector.Connection?.SinkConnectors.Remove(lastConnector);
            ConnectorViewModels.Remove(lastConnector);
            ((RelayCommand)RemoveInputCommand).RaiseCanExecuteChanged();
            ((RelayCommand)AddInputCommand).RaiseCanExecuteChanged();
        }

        public override void ResetSettingsTo(ILogicElement model)
        {
            
        }

        public void ApplySettings()
        {
            
        }
    }
}