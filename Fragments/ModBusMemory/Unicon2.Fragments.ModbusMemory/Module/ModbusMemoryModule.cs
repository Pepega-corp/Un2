using Unicon2.Fragments.ModbusMemory.Editor;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Fragments.ModbusMemory.Infrastructure.ViewModels;
using Unicon2.Fragments.ModbusMemory.Model;
using Unicon2.Fragments.ModbusMemory.ViewModels;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.ModbusMemory.Module
{
    public class ModbusMemoryModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register(typeof(IModbusMemory), typeof(Model.ModbusMemory));
            container.Register(typeof(IMemoryConversionParameters), typeof(MemoryConversionParameters));
            container.Register(typeof(IModbusMemoryEntity), typeof(ModbusMemoryEntity));
            container.Register(typeof(IModbusMemorySettings), typeof(ModbusMemorySettings));
            container.Register(typeof(IMemoryBitViewModel), typeof(MemoryBitViewModel));
            container.Register(typeof(IModbusEntityEditingViewModel), typeof(ModbusEntityEditingViewModel));
            container.Register(typeof(IFragmentEditorViewModel), typeof(ModbusMemoryEditorViewModel), 
                ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL);

            container.Register(typeof(IFragmentViewModel), typeof(ModbusMemoryViewModel),
                ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register(typeof(IModbusMemorySettingsViewModel), typeof(ModbusMemorySettingsViewModel));
            container.Register(typeof(IModbusMemoryViewModel), typeof(ModbusMemoryViewModel));
            container.Register(typeof(IModbusConversionParametersViewModel),
                typeof(ModbusConversionParametersViewModel));
            container.Register(typeof(IModbusMemoryEntityViewModel), typeof(ModbusMemoryEntityViewModel));

            //регистрация ресурсов
            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/ModbusMemoryTemplates.xaml", GetType().Assembly);

            ISerializerService serializerService = container.Resolve<ISerializerService>();
            serializerService.AddKnownTypeForSerialization(typeof(Model.ModbusMemory));
            serializerService.AddNamespaceAttribute("modbusMemory", "ModbusMemoryNS");
        }
    }
}
