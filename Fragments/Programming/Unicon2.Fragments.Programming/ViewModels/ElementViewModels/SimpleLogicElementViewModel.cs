using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public abstract class SimpleLogicElementViewModel : LogicElementViewModel, ISettingsApplicable
    {
        private List<ConnectorViewModel> _addInputConnectors = new List<ConnectorViewModel>();
        private List<ConnectorViewModel> _removeInputConnectors = new List<ConnectorViewModel>();
        
        public ICommand AddInputCommand { get; }
        public ICommand RemoveInputCommand { get; }
        public int Width => 20;
        public int Height => Inputs.Count * 10 + 10;
        /// <summary>
        /// Inputs for scheme
        /// </summary>
        public ObservableCollection<ConnectorViewModel> Inputs { get; }
        /// <summary>
        /// Outputs for scheme
        /// </summary>
        public ObservableCollection<ConnectorViewModel> Outputs { get; }
        
        public ObservableCollection<ConnectorViewModel> InputsForSettings { get; }
        public ObservableCollection<ConnectorViewModel> OutputsForSettings { get; }

        protected SimpleLogicElementViewModel(LogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _logicElementModel = model;
            this._globalCommands = globalCommands;
            AddInputCommand = new RelayCommand(AddInput, CanAddInput);
            RemoveInputCommand = new RelayCommand(RemoveInput, CanRemove);
            Inputs = new ObservableCollection<ConnectorViewModel>();
            Outputs = new ObservableCollection<ConnectorViewModel>();
            InputsForSettings = new ObservableCollection<ConnectorViewModel>();
            OutputsForSettings = new ObservableCollection<ConnectorViewModel>();
            this.ConnectorViewModels = new ObservableCollection<ConnectorViewModel>();
            this.ConnectorViewModels.CollectionChanged += OnConnectorsCollectionChanged;
            SetModel(model);
        }
        
        private void OnConnectorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in eventArgs.NewItems.Cast<ConnectorViewModel>())
                    {
                        AddConnector(c);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach (var c in eventArgs.OldItems.Cast<ConnectorViewModel>())
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

        private void AddConnector(ConnectorViewModel connector)
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

        private void RemoveConnector(ConnectorViewModel connector)
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
            ConnectorViewModel connector;
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
            ConnectorViewModel connector;
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

        public override void ResetSettingsTo(LogicElement model)
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

            for (var i = 0; i < Inputs.Count; i++)
            {
                Inputs[i].ConnectorType = InputsForSettings[i].ConnectorType;
            }
            
            for (var i = 0; i < Outputs.Count; i++)
            {
                Outputs[i].ConnectorType = OutputsForSettings[i].ConnectorType;
            }
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
            ResetBuffers();
            
            foreach (var input in Inputs)
            {
                InputsForSettings.Add(new ConnectorViewModel(this, input.Orientation, input.ConnectorType));
            }

            foreach (var output in Outputs)
            {
                OutputsForSettings.Add(new ConnectorViewModel(this, output.Orientation, output.ConnectorType));
            }
            
            base.OpenPropertyWindow();
        }
    }
}