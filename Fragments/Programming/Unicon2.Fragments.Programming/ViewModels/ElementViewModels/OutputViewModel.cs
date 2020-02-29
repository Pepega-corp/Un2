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
    public class OutputViewModel : LogicElementViewModel
    {
        private string _selectedSignal;

        public OutputViewModel(IApplicationGlobalCommands globalCommands) : base(ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL, globalCommands)
        {
            ElementName = "Выход";
            Description = "Елемент выходного дискретного сигнала";
            Symbol = "Out";

            ConnectorViewModels = new ObservableCollection<IConnectorViewModel>
            {
                new ConnectorViewModel(this, ConnectorOrientation.LEFT, ConnectorType.DIRECT)
            };

            this.OutputSignals = new ObservableCollection<string>();
        }

        public ObservableCollection<string> OutputSignals { get; }

        public string SelectedSignal
        {
            get { return this._selectedSignal; }
            set
            {
                this._selectedSignal = value;
                RaisePropertyChanged();
            }
        }
        
        protected override ILogicElement GetModel()
        {
            var output = (IOutput) _model;
            output.OutputSignalNum = this.OutputSignals.IndexOf(this.SelectedSignal);
            output.Connectors[0] = ConnectorViewModels[0].Model;

            return output;
        }

        protected override void SetModel(object modelObj)
        {
            if (!(modelObj is IOutput output))
                return;

            _model = output;

            this.OutputSignals.AddCollection(output.OutputSignals);
            this.SelectedSignal = this.OutputSignals[output.OutputSignalNum];
            ConnectorViewModels[0].Model = output.Connectors.First();
        }

        public override object Clone()
        {
            OutputViewModel ret =
                new OutputViewModel(_globalCommands);
            var newModel = new Output();
            newModel.CopyValues(_model);
            ret.Model = newModel;
            ret.Caption = this.Caption;

            for (int i = 0; i < ConnectorViewModels.Count; i++)
            {
                var connector = ConnectorViewModels[i];
                ret.ConnectorViewModels.Add(new ConnectorViewModel(connector.ParentViewModel, connector.Orientation, connector.ConnectorType));
            }

            return ret;
        }
    }
}
