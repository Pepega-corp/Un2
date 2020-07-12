using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        private string _selectedBase;
        private string _selectedSignal;
        private Input _inputModel;

        public override string StrongName =>ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;


        public InputViewModel()
        {
            _inputModel= new Input();
            _model = _inputModel;

            this.ElementName = "Вход";
            this.Description = "Елемент входного дискретного сигнала";
            this.Symbol = "In";
            Signals = new ObservableCollection<string>();
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
        }

        public InputViewModel(IApplicationGlobalCommands globalCommands) : this()
        {
            _globalCommands = globalCommands;
        }

        public List<string> Bases
        {
            get => _inputModel.Bases;
            set
            {
                _inputModel.Bases.Clear();
                if (value == null)
                    return;

                _inputModel.Bases.AddRange(value);
                RaisePropertyChanged();
            }
        }

        private List<Dictionary<int, string>> AllInputSignals
        {
            get => _inputModel.AllInputSignals;
            set
            {
                this._inputModel.AllInputSignals.Clear();
                if (value == null)
                    return;
                
                this._inputModel.AllInputSignals.AddRange(value);
            }
        }

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

                _inputModel.BaseNum = BaseIndex;

                RaisePropertyChanged();
            }
        }

        private int BaseIndex => this.Bases.IndexOf(this.SelectedBase);

        public string SelectedSignal
        {
            get => this._selectedSignal;
            set
            {
                if (string.Equals(this._selectedSignal, value))
                    return;

                this._selectedSignal = value;

                _inputModel.InputSignalNum = this.AllInputSignals[BaseIndex].First(s => s.Value == this.SelectedSignal).Key;

                RaisePropertyChanged();
            }
        }
        
        protected override ILogicElement GetModel()
        {
            _inputModel.Connectors[0] = ConnectorViewModels[0].Model;
            return _inputModel;
        }

        protected override void SetModel(ILogicElement model)
        {
            if (!(model is IInput input))
                return;

            this.AllInputSignals = input.AllInputSignals;
            this.Bases = input.Bases;
            this.SelectedBase = this.Bases[input.BaseNum];
            this.SetSignalsCollection(input.BaseNum);
            this.SelectedSignal = this.Signals[input.InputSignalNum];

            base.SetModel(model);
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
            var cloned =  (InputViewModel)Clone<InputViewModel, Input>();
            cloned._globalCommands = this._globalCommands;
            return cloned;
        }
    }
}
