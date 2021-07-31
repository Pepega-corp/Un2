using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OrViewModel : SimpleLogicElementViewModel
    {
        public override string StrongName => ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public OrViewModel(LogicElement model, IApplicationGlobalCommands globalCommands) : base(model, globalCommands)
        {
            this.ElementName = "ИЛИ";
            this.Description = "Логический элемент ИЛИ";
            this.Symbol = "|";
            SetModel(_logicElementModel);
        }

        public override LogicElementViewModel Clone()
        {
            var model = new Or();
            model.CopyValues(GetModel());
            return new OrViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
