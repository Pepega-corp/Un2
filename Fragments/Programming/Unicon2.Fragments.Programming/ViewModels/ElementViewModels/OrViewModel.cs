using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OrViewModel : LogicElementViewModel
    {
        private Or _model;

        public override string StrongName => ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public OrViewModel()
        {
            _model = new Or();
            _logicElementModel = _model;

            this.ElementName = "ИЛИ";
            this.Description = "Логический элемент ИЛИ";
            this.Symbol = "|";
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
        }

        public OrViewModel(IApplicationGlobalCommands globalCommands) : this()
        {
            _globalCommands = globalCommands;
        }

        public override ILogicElementViewModel Clone()
        {
            return (OrViewModel)Clone<OrViewModel, Or>();
        }
    }
}
