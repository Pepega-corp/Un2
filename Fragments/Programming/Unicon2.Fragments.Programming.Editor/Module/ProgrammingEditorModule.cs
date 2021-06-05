using Unicon2.Fragments.Programming.Editor.Models;
using Unicon2.Fragments.Programming.Editor.Models.LibraryElements;
using Unicon2.Fragments.Programming.Editor.ViewModel;
using Unicon2.Fragments.Programming.Editor.ViewModel.ElementEditorViewModels;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.EditorElements;
using Unicon2.Fragments.Programming.Infrastructure.ViewModels.Scheme.ElementEditorViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Programming.Editor.Module
{
    public class ProgrammingEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IProgrammModelEditor, ProgrammModelEditor>();

            container.Register<IFragmentEditorViewModel, ProgrammingEditorViewModel>(ProgrammingKeys.PROGRAMMING + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            //All models
            container.Register<ILibraryElement, InputEditor>(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, OutputEditor>(ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, InversionEditor>(ProgrammingKeys.INVERSION + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, AndEditor>(ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, OrEditor>(ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, XorEditor>(ProgrammingKeys.XOR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, AlarmJournalEditor>(ProgrammingKeys.ALARM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, SystemJournalEditor>(ProgrammingKeys.SYSTEM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            container.Register<ILibraryElement, TimerEditor>(ProgrammingKeys.TIMER + ApplicationGlobalNames.CommonInjectionStrings.EDITOR);
            //All view models
            container.Register<ILogicElementEditorViewModel, InputEditorViewModel>(ProgrammingKeys.INPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, OutputEditorViewModel>(ProgrammingKeys.OUTPUT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, InversionEditorViewModel>(ProgrammingKeys.INVERSION + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, AndEditorViewModel>(ProgrammingKeys.AND + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, OrEditorViewModel>(ProgrammingKeys.OR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, XorEditorViewModel>(ProgrammingKeys.XOR + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, AlarmJournalEditorViewModel>(ProgrammingKeys.ALARM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, SystemJournalViewModel>(ProgrammingKeys.SYSTEM_JOURNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<ILogicElementEditorViewModel, TimerViewModel>(ProgrammingKeys.TIMER + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/ProgrammingEditorTemplate.xaml", GetType().Assembly);
        }
    }
}
