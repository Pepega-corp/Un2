using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class TimerViewModel : LogicElementViewModel
    {
        private readonly Timer _model;

        public override string StrongName => ProgrammingKeys.TIMER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public TimerViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            this._globalCommands = globalCommands;
            this._model = (Timer)model;
            this._logicElementModel = model;
            
            this.ElementName = "Таймер";
            this.Description = "Логический элемент Таймер";
            this.Symbol = "T";
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            this.SetModel(this._model);
        }

        public string[] TimerTypes => this._model.TimerTypes;

        public IConnectorViewModel Input => ConnectorViewModels.First(c => c.Orientation == ConnectorOrientation.LEFT);
        public IConnectorViewModel Output => ConnectorViewModels.First(c => c.Orientation == ConnectorOrientation.RIGHT);

        public int SelectedTypeIndex
        {
            get => this._model.SelectedTypeIndex;
            set
            {

                this._model.SelectedTypeIndex = value;
                RaisePropertyChanged();
            }
        }

        public string Time
        {
            get => this._model.Time.ToString("F2", CultureInfo.CurrentCulture);
            set
            {
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var time))
                {
                    this._model.Time = time;
                    RaisePropertyChanged();
                }
            }
        }

        protected override ILogicElement GetModel()
        {
            this._model.Connectors[0] = ConnectorViewModels[0].Model;
            this._model.Connectors[1] = ConnectorViewModels[1].Model;
            return this._model;
        }

        protected override void SetModel(ILogicElement model)
        {
            if (model is Timer timer)
            {
                UpdateProperties(timer);
                base.SetModel(model);
            }
        }

        private void UpdateProperties(Timer timer)
        {
            this.SelectedTypeIndex = timer.SelectedTypeIndex;
            this.Time = timer.Time.ToString("F2", CultureInfo.CurrentCulture);
        }

        public override void ResetSettingsTo(ILogicElement model)
        {
            if (model is Timer timer)
            {
                UpdateProperties(timer);
            }
        }

        public override ILogicElementViewModel Clone()
        {
            var model = new Timer();
            model.CopyValues(GetModel());
            return new TimerViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
