using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Input;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Commands;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class AndViewModel : LogicElementViewModel
    {
        private readonly And _model;

        public override string StrongName => ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public ObservableCollection<IConnectorViewModel> Inputs { get; }
        public ObservableCollection<IConnectorViewModel> Outputs { get; }
        public int Width => 20;
        public int Heigth => Inputs.Count * 10 + 10;
        public ICommand AddInputCommand { get; }
        public ICommand RemoveInputCommand { get; }

        public AndViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            this._globalCommands = globalCommands;
            this._model = (And)model;
            this._logicElementModel = _model;

            this.ElementName = "И";
            this.Description = "Логический элемент И";
            this.Symbol = "&";
            this.Inputs = new ObservableCollection<IConnectorViewModel>();
            this.Outputs = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels.CollectionChanged += OnConnectorsCollectionChanged;

            AddInputCommand = new RelayCommand(AddInput, CanAddInput);
            RemoveInputCommand = new RelayCommand(RemoveInput, CanRemove);

            this.SetModel(this._model);
        }
        
        private void OnConnectorsCollectionChanged(object sender, NotifyCollectionChangedEventArgs eventArgs)
        {
            switch (eventArgs.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    foreach (var c in eventArgs.NewItems.Cast<IConnectorViewModel>())
                    {
                        OnAddConnector(c);
                    }
                    break;
                case NotifyCollectionChangedAction.Remove:
                    foreach(var c in eventArgs.OldItems.Cast<IConnectorViewModel>())
                    {
                        OnRemoveConnector(c);
                    }
                    break;
                case NotifyCollectionChangedAction.Reset:
                    Inputs.Clear();
                    Outputs.Clear();
                    break;
            }

            RaisePropertyChanged(nameof(Heigth));
            RaisePropertyChanged(nameof(Width));
        }

        private void OnAddConnector(IConnectorViewModel connector)
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

        private void OnRemoveConnector(IConnectorViewModel connector)
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
            if(lastConnector.Connection != null)
            {
                lastConnector.Connection.SinkConnectors.Remove(lastConnector);
            }
            ConnectorViewModels.Remove(lastConnector);
            ((RelayCommand)RemoveInputCommand).RaiseCanExecuteChanged();
            ((RelayCommand)AddInputCommand).RaiseCanExecuteChanged();
        }

        public override ILogicElementViewModel Clone()
        {
            var model = new And();
            model.CopyValues(this._model);
            return new AndViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
