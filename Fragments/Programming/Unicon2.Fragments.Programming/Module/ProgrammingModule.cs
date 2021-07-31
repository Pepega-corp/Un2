using Unicon2.Fragments.Programming.Factories;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Model;
using Unicon2.Fragments.Programming.Model.Elements;
using Unicon2.Fragments.Programming.Other;
using Unicon2.Fragments.Programming.ViewModels;
using Unicon2.Fragments.Programming.ViewModels.ElementViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Module
{
    public class ProgrammingModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<ProgramModel>();
            container.Register<IFragmentViewModel, ProgrammingViewModel>(ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicDeviceProvider>();
            container.Register<SchemeTabViewModel>();
            container.Register<LogicElementsFactory>();

            container.Register<LogicElement, Input>(ProgrammingKeys.INPUT);
            container.Register<LogicElementViewModel, InputViewModel>(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, Output>(ProgrammingKeys.OUTPUT);
            container.Register<LogicElementViewModel, OutputViewModel>(ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, Inversion>(ProgrammingKeys.INVERSION);
            container.Register<LogicElementViewModel, InversionViewModel>(ProgrammingKeys.INVERSION + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, And>(ProgrammingKeys.AND);
            container.Register<LogicElementViewModel, AndViewModel>(ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, Or>(ProgrammingKeys.OR);
            container.Register<LogicElementViewModel, OrViewModel>(ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, Xor>(ProgrammingKeys.XOR);
            container.Register<LogicElementViewModel, XorViewModel>(ProgrammingKeys.XOR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, AlarmJournal>(ProgrammingKeys.ALARM_JOURNAL);
            container.Register<LogicElementViewModel, AlarmJournalViewModel>(ProgrammingKeys.ALARM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, SystemJournal>(ProgrammingKeys.SYSTEM_JOURNAL);
            container.Register<LogicElementViewModel, SystemJournalViewModel>(ProgrammingKeys.SYSTEM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, Timer>(ProgrammingKeys.TIMER);
            container.Register<LogicElementViewModel, TimerViewModel>(ProgrammingKeys.TIMER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, RsTrigger>(ProgrammingKeys.RS_TRIGGER);
            container.Register<LogicElementViewModel, RsTriggerViewModel>(ProgrammingKeys.RS_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<LogicElement, SrTrigger>(ProgrammingKeys.SR_TRIGGER);
            container.Register<LogicElementViewModel, SrTriggerViewModel>(ProgrammingKeys.SR_TRIGGER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("UI/ProgrammingViewTemplate.xaml", GetType().Assembly);
        }
    }
}
