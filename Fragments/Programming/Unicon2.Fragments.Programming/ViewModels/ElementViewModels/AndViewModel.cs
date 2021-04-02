using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class AndViewModel : LogicElementViewModel
    {
        private And _model;

        public override string StrongName => ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public ObservableCollection<IConnectorViewModel> Inputs { get; }
        public ObservableCollection<IConnectorViewModel> Outputs { get; }
        public int Width => 20;
        public int Heigth => Inputs.Count * 10 + 10;

        public AndViewModel()
        {
            _model = new And();
            _logicElementModel = _model;

            this.ElementName = "И";
            this.Description = "Логический элемент И";
            this.Symbol = "&";
            this.Inputs = new ObservableCollection<IConnectorViewModel>();
            this.Outputs = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            this.ConnectorViewModels.CollectionChanged += OnConnectorsCollectionChanged;
        }

        public AndViewModel(IApplicationGlobalCommands globalCommands) : this()
        {
            _globalCommands = globalCommands;
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
                    foreach(var c in eventArgs.OldItems.Cast<IConnectorViewModel>())
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

        public override ILogicElementViewModel Clone()
        {
            return (AndViewModel)Clone<AndViewModel, And>();
        }
    }
}
