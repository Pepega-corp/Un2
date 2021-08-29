using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OutputViewModel : LogicElementViewModel
    {
        private string _selectedSignal;
        private readonly Output _outputModel;

        public OutputViewModel(LogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _globalCommands = globalCommands;
            _outputModel = (Output) model;
            _logicElementModel = _outputModel;
            ElementName = "Выход";
            Description = "Елемент выходного дискретного сигнала";
            Symbol = "Out";
            ConnectorViewModels = new ObservableCollection<ConnectorViewModel>();
            OutputSignals = new ObservableCollection<string>();
            SetModel(this._outputModel);
        }

        public override string StrongName => ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public ObservableCollection<string> OutputSignals { get; }

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

        protected override LogicElement GetModel()
        {
            _outputModel.Connectors[0] = ConnectorViewModels[0].Model;
            return _outputModel;
        }

        protected override void SetModel(LogicElement model)
        {
            if (model is Output output)
            {
                UpdateProperties(output);
                base.SetModel(model);
            }
        }

        private void UpdateProperties(Output output)
        {
            this.OutputSignals.Clear();
            this.OutputSignals.AddCollection(output.OutputSignals);
            this.SelectedSignal = this.OutputSignals[output.OutputSignalNum];
        }

        public override void ResetSettingsTo(LogicElement model)
        {
            if (model is Output output)
            {
                UpdateProperties(output);
            }
        }

        public override LogicElementViewModel Clone()
        {
            var model = new Output();
            model.CopyValues(GetModel());
            return new OutputViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
