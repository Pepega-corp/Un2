using Unicon2.Fragments.Measuring.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Factories;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Address;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel;
using Unicon2.Fragments.Measuring.Infrastructure.ViewModel.Elements;
using Unicon2.Fragments.Measuring.Model;
using Unicon2.Fragments.Measuring.Model.Address;
using Unicon2.Fragments.Measuring.Model.Elements;
using Unicon2.Fragments.Measuring.ViewModel;
using Unicon2.Fragments.Measuring.ViewModel.Elements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Measuring.Module
{
    public class MeasuringModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IMeasuringMonitor, MeasuringMonitor>();
            container.Register<IFragmentViewModel, MeasuringMonitorViewModel>(MeasuringKeys.MEASURING_MONITOR + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IAnalogMeasuringElement, AnalogMeasuringElement>();
            container.Register<IDiscretMeasuringElement, DescretMeasuringElement>();
            container.Register<IControlSignal, ControlSignal>();

            container.Register<IMeasuringGroup, MeasuringGroup>();
            container.Register<IMeasuringElement, DescretMeasuringElement>(MeasuringKeys.DISCRET_MEASURING_ELEMENT);
            container.Register<IMeasuringElement, AnalogMeasuringElement>(MeasuringKeys.ANALOG_MEASURING_ELEMENT);
            container.Register<IMeasuringElementViewModel, DiscretMeasuringElementViewModel>(MeasuringKeys.DISCRET_MEASURING_ELEMENT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IMeasuringElementViewModel, AnalogMeasuringElementViewModel>(MeasuringKeys.ANALOG_MEASURING_ELEMENT + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IMeasuringElementViewModel, ControlSignalViewModel>(MeasuringKeys.CONTROL_SIGNAL + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register<IAddressOfBit, AddressOfBit>();
            container.Register<IMeasuringElementViewModelFactory, MeasuringElementViewModelFactory>();
            container.Register<IMeasuringElementFactory, MeasuringElementFactory>();
            container.Register<IMeasuringGroupViewModel, MeasuringGroupViewModel>();
            container.Register<IMeasuringElement, ControlSignal>(MeasuringKeys.CONTROL_SIGNAL);


            container.Register<IWritingValueContext, WritingValueContext>();
            container.Register<IAddressOfBit, AddressOfBit>();
            container.Register<IMeasuringElementViewModelFactory, MeasuringElementViewModelFactory>();
            container.Register<IMeasuringElementFactory, MeasuringElementFactory>();
            container.Register<IMeasuringGroupViewModelFactory, MeasuringGroupViewModelFactory>();
        
            //ISerializerService serializerService = container.Resolve<ISerializerService>();
            //serializerService.AddKnownTypeForSerializationRange(new[] { typeof(MeasuringMonitor), typeof(MeasuringGroup), typeof(AnalogMeasuringElement), typeof(DescretMeasuringElement), typeof(AddressOfBit), typeof(AddressOfBit), typeof(ControlSignal), typeof(WritingValueContext) });
            //serializerService.AddNamespaceAttribute("measuringMonitor", "MeasuringMonitorNS");
            //serializerService.AddNamespaceAttribute("measuringGroup", "MeasuringGroupNS");
            //serializerService.AddNamespaceAttribute("analogMeasuringElement", "AnalogMeasuringElementNS");
            //serializerService.AddNamespaceAttribute("descretMeasuringElement", "DescretMeasuringElementNS");
            //serializerService.AddNamespaceAttribute("addressOfBit", "AddressOfBitNS");
            //serializerService.AddNamespaceAttribute("controlSignal", "ControlSignalNS");
            //serializerService.AddNamespaceAttribute("writingValueContext", "WritingValueContextNS");

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/MeasuringDataTemplates.xaml",
                this.GetType().Assembly);
        }
    }
}
