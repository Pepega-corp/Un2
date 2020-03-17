using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;

namespace Unicon2.Fragments.ModbusMemory.Editor
{
    public class ModbusMemoryEditorViewModel : IFragmentEditorViewModel, IFragmentViewModel
    {
        private IModbusMemory _model;

        public ModbusMemoryEditorViewModel(IModbusMemory modbusMemory)
        {
            this._model = modbusMemory;
        }


        public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY + ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;
		
        public string NameForUiKey => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY;
        public IDeviceFragment BuildDeviceFragment()
        {
	        return _model;
        }

        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }
        public void Initialize(IDeviceFragment deviceFragment)
        {
	       
        }
    }
}
