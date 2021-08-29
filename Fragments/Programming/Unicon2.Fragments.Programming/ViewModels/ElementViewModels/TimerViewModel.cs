﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class TimerViewModel : LogicElementViewModel, ISettingsApplicable
    {
        private readonly Timer _model;
        private double _time;
        private int _selectedIndex;

        public override string StrongName => ProgrammingKeys.TIMER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        

        public TimerViewModel(LogicElement model, IApplicationGlobalCommands globalCommands)
        {
            this._globalCommands = globalCommands;
            this._model = (Timer)model;
            this._logicElementModel = model;
            
            this.ElementName = "Таймер";
            this.Description = "Логический элемент Таймер";
            this.Symbol = "T";
            this.ConnectorViewModels = new ObservableCollection<ConnectorViewModel>();
            this.SetModel(this._model);
        }

        public string[] TimerTypes => this._model.TimerTypes;

        public ConnectorViewModel Input => ConnectorViewModels.First(c => c.Orientation == ConnectorOrientation.LEFT);
        public ConnectorViewModel InputForSetting { get; private set; }
        public ConnectorViewModel Output => ConnectorViewModels.First(c => c.Orientation == ConnectorOrientation.RIGHT);
        public ConnectorViewModel OutputForSetting { get; private set; }

        public int SelectedTypeIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                RaisePropertyChanged();
            }
        }

        public string Time
        {
            get => _time.ToString("F2", CultureInfo.CurrentCulture);
            set
            {
                if (double.TryParse(value, NumberStyles.Float, CultureInfo.CurrentCulture, out var time))
                {
                    _time = time;
                    RaisePropertyChanged();
                }
            }
        }

        protected override LogicElement GetModel()
        {
            this._model.Connectors[0] = ConnectorViewModels[0].Model;
            this._model.Connectors[1] = ConnectorViewModels[1].Model;
            return this._model;
        }

        protected override void SetModel(LogicElement model)
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

        public override LogicElementViewModel Clone()
        {
            var model = new Timer();
            model.CopyValues(GetModel());
            return new TimerViewModel(model, _globalCommands) { Caption = this.Caption };
        }

        public void ApplySettings()
        {
            Input.ConnectorType = InputForSetting.ConnectorType;
            Output.ConnectorType = OutputForSetting.ConnectorType;
            this._model.Time = _time;
            this._model.SelectedTypeIndex = _selectedIndex;
        }
        
        public override void ResetSettingsTo(LogicElement model)
        {
            if (model is Timer timer)
            {
                UpdateProperties(timer);
            }
        }

        public override void OpenPropertyWindow()
        {
            InputForSetting = new ConnectorViewModel(this, new Connector(Input.Model));
            RaisePropertyChanged(nameof(InputForSetting));
            OutputForSetting = new ConnectorViewModel(this, new Connector(Output.Model));
            RaisePropertyChanged(nameof(OutputForSetting));
            base.OpenPropertyWindow();
        }
    }
}
