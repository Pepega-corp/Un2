using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class SystemJournalViewModel : LogicElementViewModel
    {
        private string _selectedSignal;
        private readonly SystemJournal _model;

        public SystemJournalViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _globalCommands = globalCommands;
            _model = (SystemJournal) model;
            _logicElementModel = _model;
            ElementName = "Журнал Системы";
            Description = "Елемент выходного дискретного сигнала Журнал Системы";
            Symbol = "ЖС";
            ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            OutputSignals = new ObservableCollection<string>();
            SetModel(this._model);
        }

        public override string StrongName => ProgrammingKeys.ALARM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public ObservableCollection<string> OutputSignals { get; }

        public string SelectedSignal
        {
            get { return this._selectedSignal; }
            set
            {
                this._selectedSignal = value;
                _model.OutputSignalNum = this.OutputSignals.IndexOf(this.SelectedSignal);
                RaisePropertyChanged();
            }
        }

        protected override ILogicElement GetModel()
        {
            _model.Connectors[0] = ConnectorViewModels[0].Model;
            return _model;
        }

        protected override void SetModel(ILogicElement model)
        {
            if (model is SystemJournal journal)
            {
                UpdateProperties(journal);
                base.SetModel(model);
            }
        }

        private void UpdateProperties(SystemJournal journal)
        {
            this.OutputSignals.Clear();
            this.OutputSignals.AddCollection(journal.OutputSignals);
            this.SelectedSignal = this.OutputSignals[journal.OutputSignalNum];
        }

        public override void ResetSettingsTo(ILogicElement model)
        {
            if (model is SystemJournal journal)
            {
                UpdateProperties(journal);
            }
        }

        public override ILogicElementViewModel Clone()
        {
            var model = new SystemJournal();
            model.CopyValues(GetModel());
            return new SystemJournalViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}