using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OutputViewModel : LogicElementViewModel
    {
        public OutputViewModel() : base(ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL)
        {
            ElementName = "Выход";
            Description = "Елемент выходного дискретного сигнала";
            Symbol = "Out";

            Connectors = new ObservableCollection<IConnectorViewModel>
            {
                new ConnectorViewModel(this, ConnectorOrientation.LEFT, ConnectorType.DIRECT)
            };
        }

        public override object Clone()
        {
            OutputViewModel ret =
                new OutputViewModel
                {
                    Model = (this.Model as ILogicElement)?.Clone(),
                    IsSelected = this.IsSelected,
                    DebugMode = this.DebugMode,
                    Caption = this.Caption,
                    ValidationError = this.ValidationError
                };

            for (int i = 0; i < Connectors.Count; i++)
            {
                var connector = Connectors[i];
                ret.Connectors.Add(new ConnectorViewModel(connector.ParentViewModel, connector.Orientation, connector.ConnectorType));
            }

            return ret;
        }
    }
}
