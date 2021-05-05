using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class SimpleLogicElementViewModel : LogicElementViewModel, ISettingsApplicable
    {
        private List<IConnectorViewModel> _addInputConnectors = new List<IConnectorViewModel>();
        private List<IConnectorViewModel> _removeInputConnectors = new List<IConnectorViewModel>();
        
        public ICommand AddInputCommand { get; }
        public ICommand RemoveInputCommand { get; }
        public int Width => 20;
        public int Height => Inputs.Count * 10 + 10;
        /// <summary>
        /// Inputs for scheme
        /// </summary>
        public ObservableCollection<IConnectorViewModel> Inputs { get; }
        /// <summary>
        /// Outputs for scheme
        /// </summary>
        public ObservableCollection<IConnectorViewModel> Outputs { get; }
        
        public ObservableCollection<IConnectorViewModel> InputsForSettings { get; }
        public ObservableCollection<IConnectorViewModel> OutputsForSettings { get; }

        protected SimpleLogicElementViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _logicElementModel = model;
            this._globalCommands = globalCommands;
            AddInputCommand = new RelayCommand(AddInput, CanAddInput);
            RemoveInputCommand = new RelayCommand(RemoveInput, CanRemove);
            Inputs = new ObservableCollection<IConnectorViewModel>();
            Outputs = new ObservableCollection<IConnectorViewModel>();
            InputsForSettings = new ObservableCollection<IConnectorViewModel>();
            OutputsForSettings = new ObservableCollection<IConnectorViewModel>();
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
            return InputsForSettings.Count < 8;
        }

        private void AddInput()
        {
            IConnectorViewModel connector;
            if (_removeInputConnectors.Count > 0)
            {
                connector = _removeInputConnectors.Last();
                _removeInputConnectors.Remove(connector);
            }
            else
            {
                connector = new ConnectorViewModel(this, ConnectorOrientation.LEFT, ConnectorType.DIRECT);
                _addInputConnectors.Add(connector);
            }
            InputsForSettings.Add(connector);
            ((RelayCommand)AddInputCommand).RaiseCanExecuteChanged();
            ((RelayCommand)RemoveInputCommand).RaiseCanExecuteChanged();
        }

        private bool CanRemove()
        {
            return InputsForSettings.Count > 2;
        }

        private void RemoveInput()
        {
            IConnectorViewModel connector;
            if (_addInputConnectors.Count > 0)
            {
                connector = _addInputConnectors.Last();
                _addInputConnectors.Remove(connector);
            }
            else
            {
                connector = InputsForSettings.Last();
                _removeInputConnectors.Add(connector);
            }

            InputsForSettings.Remove(connector);
            ((RelayCommand)RemoveInputCommand).RaiseCanExecuteChanged();
            ((RelayCommand)AddInputCommand).RaiseCanExecuteChanged();
        }

        public override void ResetSettingsTo(ILogicElement model)
        {
            ResetBuffers();
        }

        public void ApplySettings()
        {
            if (_addInputConnectors.Count > 0)
            {
                ConnectorViewModels.AddCollection(_addInputConnectors);
            }
            else if (_removeInputConnectors.Count > 0)
            {
                foreach (var removeConnector in _removeInputConnectors)
                {
                    removeConnector.Connection?.SinkConnectors.Remove(removeConnector);
                    ConnectorViewModels.Remove(removeConnector);
                }
            }
            
            ResetBuffers();
        }

        private void ResetBuffers()
        {
            _addInputConnectors.Clear();
            _removeInputConnectors.Clear();
            
            InputsForSettings.Clear();
            OutputsForSettings.Clear();
        }

        public override void OpenPropertyWindow()
        {
            InputsForSettings.AddCollection(Inputs);
            OutputsForSettings.AddCollection(Outputs);
            base.OpenPropertyWindow();
        }
    }
}