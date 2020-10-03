using Unicon2.Fragments.Measuring.Editor.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.Factories;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.Interfaces.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Editor.ViewModel;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Address;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Dependencies;
using Unicon2.Fragments.Measuring.Editor.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Editor.Module
{
    public class MeasuringEditorModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IFragmentEditorViewModel), typeof(MeasuringMonitorEditorViewModel),
                MeasuringKeys.MEASURING_MONITOR +
                ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<IMeasuringGroupEditorViewModel, MeasuringGroupEditorViewModel>();
            container.Register<IMeasuringElementEditorViewModel, AnalogMeasuringElementEditorViewModel>(MeasuringKeys.ANALOG_MEASURING_ELEMENT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<IMeasuringElementEditorViewModel, DiscretMeasuringElementEditorViewModel>(MeasuringKeys.DISCRET_MEASURING_ELEMENT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<IMeasuringElementEditorViewModel, ControlSignalEditorViewModel>(MeasuringKeys.CONTROL_SIGNAL + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);
            container.Register<IMeasuringElementEditorViewModel, DateTimeMeasuringEditorViewModel>(MeasuringKeys.DATE_TIME_ELEMENT + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Register<IBitAddressEditorViewModel, BitAddressEditorViewModel>();
            container.Register<IMeasuringElementEditorViewModelFactory, MeasuringElementEditorViewModelFactory>();
            container.Register<IMeasuringGroupEditorViewModelFactory, MeasuringGroupEditorViewModelFactory>();
            container.Register<IWritingValueContextViewModel, WritingValueContextViewModel>();
         

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/MeasuringDataTemplates.xaml", GetType().Assembly);
        }
    }
}
