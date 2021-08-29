using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class XorViewModel : SimpleLogicElementViewModel
    {
        public XorViewModel(LogicElement model, IApplicationGlobalCommands globalCommands) : base(model, globalCommands)
        {
            this.ElementName = "Искл. ИЛИ";
            this.Description = "Логический элемент исключающее ИЛИ";
            this.Symbol = "^";
            SetModel(_logicElementModel);
        }

        public override string StrongName => ProgrammingKeys.XOR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;
        public override LogicElementViewModel Clone()
        {
            var model = new Xor();
            model.CopyValues(GetModel());
            return new XorViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}