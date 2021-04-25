using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OutputViewModel : LogicElementViewModel
    {
        private string _selectedSignal;
        private readonly Output _outputModel;

        public OutputViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _globalCommands = globalCommands;
            _outputModel = (Output) model;
            _logicElementModel = _outputModel;
            ElementName = "Выход";
            Description = "Елемент выходного дискретного сигнала";
            Symbol = "Out";
            ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
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

        protected override ILogicElement GetModel()
        {
            _outputModel.Connectors[0] = ConnectorViewModels[0].Model;
            return _outputModel;
        }

        protected override void SetModel(ILogicElement model)
        {
            if (model is IOutput output)
            {
                this.OutputSignals.Clear();
                this.OutputSignals.AddCollection(output.OutputSignals);
                this.SelectedSignal = this.OutputSignals[output.OutputSignalNum];
                base.SetModel(model);
            }
        }

        public override ILogicElementViewModel Clone()
        {
            var model = new Output();
            model.CopyValues(this._outputModel);
            return new OutputViewModel(model, _globalCommands);
        }
    }
}
