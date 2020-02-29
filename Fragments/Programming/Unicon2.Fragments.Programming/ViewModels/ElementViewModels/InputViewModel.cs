using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
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

            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>
            {
                new ConnectorViewModel(this, ConnectorOrientation.RIGHT, ConnectorType.DIRECT)
            };

            this.Bases = new ObservableCollection<string>();
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
                this.SetSignalsCollection(Bases.IndexOf(_selectedBase));
                this.SelectedSignal = this.Signals[0];
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
        
        protected override ILogicElement GetModel()
        {
            var inputModel = (IInput) _model;

            var baseIndex =  this.Bases.IndexOf(this.SelectedBase);
            inputModel.BaseNum = baseIndex;
            inputModel.InputSignalNum = this._allInputSignals[baseIndex].First(s => s.Value == this.SelectedSignal).Key;
            inputModel.Connectors[0] = ConnectorViewModels[0].Model;

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

            ConnectorViewModels[0].Model = model.Connectors.First();
        }

        private void SetSignalsCollection(int index)
        {
            this.Signals.Clear();

            if(index == -1)
                return;

            var selectedSignals = this._allInputSignals[index].Select(s => s.Value).ToArray();
            this.Signals.AddCollection(selectedSignals);
        }

        public override object Clone()
        {
            InputViewModel ret =
                new InputViewModel(this._globalCommands);
            var newModel = new Input();
            newModel.CopyValues(_model);
            ret.Model = newModel;
            ret.Caption = this.Caption;

            ret.ConnectorViewModels.Clear();
            for (int i = 0; i < this.ConnectorViewModels.Count; i++)
            {
                IConnectorViewModel connector = this.ConnectorViewModels[i];
                ret.ConnectorViewModels.Add(new ConnectorViewModel(connector.ParentViewModel, connector.Orientation, connector.ConnectorType));
            }

            return ret;
        }
    }
}
