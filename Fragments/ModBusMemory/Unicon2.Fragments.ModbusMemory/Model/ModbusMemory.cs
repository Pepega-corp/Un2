using Newtonsoft.Json;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.ModbusMemory.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ModbusMemory : Disposable, IModbusMemory
	{
		public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY;

		public IFragmentSettings FragmentSettings { get; set; }
	}
}
