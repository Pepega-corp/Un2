using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class AlarmJournalViewModel : LogicElementViewModel
    {
        private string _selectedSignal;
        private readonly AlarmJournal _model;

        public AlarmJournalViewModel(LogicElement model, IApplicationGlobalCommands globalCommands)
        {
            _globalCommands = globalCommands;
            _model = (AlarmJournal) model;
            _logicElementModel = _model;
            ElementName = "Журнал Аварий";
            Description = "Елемент выходного дискретного сигнала Журнал Аварий";
            Symbol = "ЖА";
            ConnectorViewModels = new ObservableCollection<ConnectorViewModel>();
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

        protected override LogicElement GetModel()
        {
            _model.Connectors[0] = ConnectorViewModels[0].Model;
            return _model;
        }

        protected override void SetModel(LogicElement model)
        {
            if (model is AlarmJournal journal)
            {
                UpdateProperties(journal);
                base.SetModel(model);
            }
        }

        private void UpdateProperties(AlarmJournal journal)
        {
            this.OutputSignals.Clear();
            this.OutputSignals.AddCollection(journal.OutputSignals);
            this.SelectedSignal = this.OutputSignals[journal.OutputSignalNum];
        }

        public override void ResetSettingsTo(LogicElement model)
        {
            if (model is AlarmJournal journal)
            {
                UpdateProperties(journal);
            }
        }

        public override LogicElementViewModel Clone()
        {
            var model = new AlarmJournal();
            model.CopyValues(GetModel());
            return new AlarmJournalViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}