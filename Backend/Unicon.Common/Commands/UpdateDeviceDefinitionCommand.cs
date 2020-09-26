using System.Collections.Generic;
using Unicon.Common.Interfaces;

namespace Unicon.Common.Commands
{
	public class UpdateDeviceDefinitionCommand:ICommand
	{
		public int? DeviceDefinitionId { get; set; }	
		public string DefinitionString { get; set; }
		public List<string> Tags { get; set; }

	}
}