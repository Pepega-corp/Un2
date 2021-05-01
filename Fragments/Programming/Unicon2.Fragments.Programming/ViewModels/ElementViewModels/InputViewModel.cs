using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class InputViewModel : LogicElementViewModel
    {
        private int _baseIndex = -1;
        private int _selectedSignalIndex = -1;
        private readonly Input _inputModel;
        private readonly List<Dictionary<int, string>> _allInputSignals;
        private readonly List<string> _bases;

        public override string StrongName =>ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        
        public InputViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            this._globalCommands = globalCommands;
            this._inputModel = (Input)model;
            this._logicElementModel = model;
            this._bases = new List<string>();
            this._allInputSignals = new List<Dictionary<int, string>>();
            this.ElementName = "Вход";
            this.Description = "Елемент входного дискретного сигнала";
            this.Symbol = "In";
            this.Signals = new ObservableCollection<string>();
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            this.SetModel(this._inputModel);
        }

        public List<string> Bases
        {
            get => _bases;
            set
            {
                _bases.Clear();
                if (value == null)
                    return;

                _bases.AddRange(value);
                RaisePropertyChanged();
            }
        }

        private List<Dictionary<int, string>> AllInputSignals
        {
            get => _allInputSignals;
            set
            {
                this._allInputSignals.Clear();
                if (value == null)
                    return;
                
                this._allInputSignals.AddRange(value);
            }
        }

        public ObservableCollection<string> Signals { get; }

        public int BaseIndex
        {
            get => _baseIndex;
            set
            {
                if(this._baseIndex == value)
                    return;

                this._baseIndex = value;
                this._inputModel.BaseNum = this._baseIndex;
                SetSignalsCollection(this._baseIndex);
                SelectedSignalIndex = 0;
                RaisePropertyChanged();
            }
        }

        public int SelectedSignalIndex
        {
            get => this._selectedSignalIndex;
            set
            {
                if (_selectedSignalIndex == value || value < 0)
                    return;
                this._selectedSignalIndex = value;
                RaisePropertyChanged();
            }
        }
        
        protected override ILogicElement GetModel()
        {
            _inputModel.Connectors[0] = ConnectorViewModels[0].Model;
            _inputModel.InputSignalNum = this.AllInputSignals[this.BaseIndex]
                        .First(s => s.Value == this.Signals[this._selectedSignalIndex]).Key;
            this._inputModel.BaseNum = this.BaseIndex;
            return _inputModel;
        }

        protected override void SetModel(ILogicElement model)
        {
            if (model is IInput input)
            {
                this.AllInputSignals = input.AllInputSignals;
                this.Bases = input.Bases;
                this.BaseIndex = input.BaseNum;
                var selectedDictionary = this.AllInputSignals[this.BaseIndex];
                var selectedSignal = selectedDictionary.First(sd => sd.Key == input.InputSignalNum).Value;
                this.SelectedSignalIndex = this.Signals.IndexOf(selectedSignal);
                base.SetModel(model);
            }
        }

        private void SetSignalsCollection(int index)
        {
            this.Signals.Clear();

            if(index == -1)
                return;

            var selectedSignals = this.AllInputSignals[index].Select(s => s.Value).ToArray();
            this.Signals.AddCollection(selectedSignals);
        }

        public override ILogicElementViewModel Clone()
        {
            var model = new Input();
            model.CopyValues(GetModel());
            return new InputViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
