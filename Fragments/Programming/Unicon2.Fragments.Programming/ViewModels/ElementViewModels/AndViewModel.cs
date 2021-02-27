using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class AndViewModel : LogicElementViewModel
    {
        private And _model;

        public override string StrongName => ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public AndViewModel()
        {
            _model = new And();
            _logicElementModel = _model;

            this.ElementName = "И";
            this.Description = "Логический элемент И";
            this.Symbol = "&";
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
        }

        public AndViewModel(IApplicationGlobalCommands globalCommands) : this()
        {
            _globalCommands = globalCommands;
        }

        public override ILogicElementViewModel Clone()
        {
            return (AndViewModel)Clone<AndViewModel, And>();
        }
    }
}
