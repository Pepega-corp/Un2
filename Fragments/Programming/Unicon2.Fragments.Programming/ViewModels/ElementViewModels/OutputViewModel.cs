using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OutputViewModel : LogicElementViewModel
    {
        private string _selectedSignal;
        private Output _outputModel;

        public OutputViewModel()
        {
            _outputModel = new Output();
            _logicElementModel = _outputModel;
            ElementName = "Выход";
            Description = "Елемент выходного дискретного сигнала";
            Symbol = "Out";
            ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
        }

        public OutputViewModel(IApplicationGlobalCommands globalCommands) : this()
        {
            _globalCommands = globalCommands;
        }

        public override string StrongName => ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public List<string> OutputSignals
        {
            get => _outputModel.OutputSignals;
            set
            {
                _outputModel.OutputSignals.Clear();

                if (value == null)
                    return;

                _outputModel.OutputSignals.AddRange(value);
            }
        }

        public string SelectedSignal
        {
            get { return this._selectedSignal; }
            set
            {
                this._selectedSignal = value;
                _outputModel.OutputSignalNum = this.OutputSignals.IndexOf(this.SelectedSignal);
                RaisePropertyChanged();
            }
        }

        protected override ILogicElement GetModel()
        {
            _outputModel.Connectors[0] = ConnectorViewModels[0].Model;
            return _outputModel;
        }

        protected override void SetModel(ILogicElement model)
        {
            if (!(model is IOutput output))
                return;

            this.OutputSignals = output.OutputSignals;
            this.SelectedSignal = this.OutputSignals[output.OutputSignalNum];

            base.SetModel(model);
        }

        public override ILogicElementViewModel Clone()
        {
            return (OutputViewModel)Clone<OutputViewModel, Output>();
        }
    }
}
