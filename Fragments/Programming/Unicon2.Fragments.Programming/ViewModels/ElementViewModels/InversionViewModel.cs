using System.Collections.ObjectModel;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class InversionViewModel: LogicElementViewModel
    {
        public override string StrongName => ProgrammingKeys.INVERSION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public InversionViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands)
        {
            base._globalCommands = globalCommands;
            _logicElementModel = (Inversion) model;

            this.ElementName = "НЕ";
            this.Description = "Елемент инверсии логического сигнала";
            this.Symbol = "~";
            this.ConnectorViewModels = new ObservableCollection<IConnectorViewModel>();
            SetModel(_logicElementModel);
        }
        
        public override ILogicElementViewModel Clone()
        {
            var model = new Inversion();
            model.CopyValues(GetModel());
            return new InversionViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
