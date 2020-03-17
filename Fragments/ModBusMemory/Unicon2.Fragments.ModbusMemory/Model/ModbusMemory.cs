using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Unicon2.Fragments.ModbusMemory.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.BaseItems;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.ModbusMemory.Model
{
	[JsonObject(MemberSerialization.OptIn)]
	public class ModbusMemory : Disposable, IModbusMemory
	{
		public string StrongName => ApplicationGlobalNames.FragmentInjectcionStrings.MODBUSMEMORY;

		public IFragmentSettings FragmentSettings { get; set; }

		public IDataProvider DataProvider { get; set; }
	}
}
