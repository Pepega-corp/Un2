using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class SrTriggerViewModel : TriggerViewModel
    {
        public override string StrongName => ProgrammingKeys.SR_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public SrTriggerViewModel(LogicElement model, IApplicationGlobalCommands globalCommands) : base(model, globalCommands)
        {
            this.ElementName = "SR-триггер";
            this.Description = "Логический элемент SR-триггер";
            this.Symbol = "SRT";
        }
        
        public override LogicElementViewModel Clone()
        {
            var model = new SrTrigger();
            model.CopyValues(GetModel());
            return new SrTriggerViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}