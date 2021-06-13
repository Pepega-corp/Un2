using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class RsTriggerViewModel : TriggerViewModel
    {
        public override string StrongName => ProgrammingKeys.RS_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public RsTriggerViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands) : base(model, globalCommands)
        {
            this.ElementName = "RS-триггер";
            this.Description = "Логический элемент RS-триггер";
            this.Symbol = "RST";
        }
        
        public override ILogicElementViewModel Clone()
        {
            var model = new RsTrigger();
            model.CopyValues(GetModel());
            return new RsTriggerViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}