using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class OrViewModel : SimpleLogicElementViewModel
    {
        public override string StrongName => ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public OrViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands) : base(model, globalCommands)
        {
            this.ElementName = "ИЛИ";
            this.Description = "Логический элемент ИЛИ";
            this.Symbol = "|";
            SetModel(_logicElementModel);
        }

        public override ILogicElementViewModel Clone()
        {
            var model = new Or();
            model.CopyValues(GetModel());
            return new OrViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
