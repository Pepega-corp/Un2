using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Unity.Common;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class InputViewModel : LogicElementViewModel
    {
        private List<Dictionary<int, string>> _allInputSignals;
        private string _selectedBase;
        private string _selectedSignal;

        public InputViewModel(IApplicationGlobalCommands globalCommands) : base(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL, globalCommands)
        {
            this.ElementName = "Вход";
            this.Description = "Елемент входного дискретного сигнала";
            this.Symbol = "In";

            this.Connectors = new ObservableCollection<IConnectorViewModel>
            {
                new ConnectorViewModel(this, ConnectorOrientation.RIGHT, ConnectorType.DIRECT)
            };

            this.Bases = new ObservableCollection<string>();
            this.Bases.CollectionChanged += this.OnBaseChanged;
            this.Signals = new ObservableCollection<string>();
        }

        public ObservableCollection<string> Bases { get; }
        public ObservableCollection<string> Signals { get; }

        public string SelectedBase
        {
            get => this._selectedBase;
            set
            {
                if(string.Equals(this._selectedBase, value))
                    return;
                this._selectedBase = value;
                RaisePropertyChanged();
            }
        }

        public string SelectedSignal
        {
            get => this._selectedSignal;
            set
            {
                if (string.Equals(this._selectedSignal, value))
                    return;
                this._selectedSignal = value;
                RaisePropertyChanged();
            }
        }

        public string Name
        {
            get => _model.Name;
            set
            {
                _model.Name = value;
                RaisePropertyChanged();
            }
        }

        protected override ILogicElement GetModel()
        {
            var inputModel = (IInput) _model;

            var baseIndex =  this.Bases.IndexOf(this.SelectedBase);
            inputModel.BaseNum = baseIndex;

            inputModel.InputSignalNum = this._allInputSignals[baseIndex].First(s => s.Value == this.SelectedSignal).Key;

            return inputModel;
        }

        protected override void SetModel(object modelObj)
        {
            if (!(modelObj is IInput model))
                return;

            this._model = model;
            this._allInputSignals = model.AllInputSignals;
            this.Bases.Clear();
            this.Bases.AddCollection(model.Bases);
            this.SelectedBase = this.Bases[model.BaseNum];
            this.SetSignalsCollection(model.BaseNum);
            this.SelectedSignal = this.Signals[model.InputSignalNum];
        }

        private void SetSignalsCollection(int index)
        {
            this.Signals.Clear();

            if(index == -1)
                return;

            var selectedSignals = this._allInputSignals[index].Select(s => s.Value).ToArray();
            this.Signals.AddCollection(selectedSignals);
        }

        private void OnBaseChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var index = e.NewStartingIndex;
            this.SetSignalsCollection(index);
        }

        public override object Clone()
        {
            InputViewModel ret =
                new InputViewModel(this._globalCommands)
                {
                    Model = (this.Model as ILogicElement)?.Clone(),
                    IsSelected = this.IsSelected,
                    DebugMode = this.DebugMode,
                    Caption = this.Caption,
                    ValidationError = this.ValidationError
                };

            ret.Connectors.Clear();
            for (int i = 0; i < this.Connectors.Count; i++)
            {
                IConnectorViewModel connector = this.Connectors[i];
                ret.Connectors.Add(new ConnectorViewModel(connector.ParentViewModel, connector.Orientation, connector.ConnectorType));
            }

            return ret;
        }

        public override void Dispose()
        {
            this.Bases.CollectionChanged -= this.OnBaseChanged;

            GC.SuppressFinalize(this);
        }
    }
}
