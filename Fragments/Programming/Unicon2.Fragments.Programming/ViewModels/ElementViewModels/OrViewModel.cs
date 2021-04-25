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
    public class OrViewModel : LogicElementViewModel
    {
        private readonly Or _model;

        public override string StrongName => ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public ObservableCollection<IConnectorViewModel> Inputs { get; }
        public ObservableCollection<IConnectorViewModel> Outputs { get; }
        public int Width => 20;
        public int Heigth => Inputs.Count * 10 + 10;
        public ICommand AddInputCommand { get; }
        public ICommand RemoveInputCommand { get; }

        public OrViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _globalCommands = globalCommands;
            _model = (Or)model;
            _logicElementModel = _model;

            this.ElementName = "ИЛИ";
            this.Description = "Логический элемент ИЛИ";
            this.Symbol = "|";
            Inputs = new ObservableCollection<IConnectorViewModel>();
            Outputs = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels.CollectionChanged += OnConnectorsCollectionChanged;

            AddInputCommand = new RelayCommand(AddInput, CanAddInput);
            RemoveInputCommand = new RelayCommand(RemoveInput, CanRemove);
            SetModel(this._model);
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

            RaisePropertyChanged(nameof(Heigth));
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

        public override ILogicElementViewModel Clone()
        {
            var model = new Or();
            model.CopyValues(this._model);
            return new OrViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
