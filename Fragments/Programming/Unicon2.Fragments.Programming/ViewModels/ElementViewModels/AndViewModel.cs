﻿using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementViewModels;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.Programming.ViewModels.ElementViewModels
{
    public class AndViewModel : SimpleLogicElementViewModel
    {
        public override string StrongName => ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public AndViewModel(ILogicElement model, IApplicationGlobalCommands globalCommands): base(model, globalCommands)
        {
            this.ElementName = "И";
            this.Description = "Логический элемент И";
            this.Symbol = "&";
            this.SetModel(_logicElementModel);
        }
        
        public override ILogicElementViewModel Clone()
        {
            var model = new And();
            model.CopyValues(GetModel());
            return new AndViewModel(model, _globalCommands) { Caption = this.Caption };
        }
    }
}
