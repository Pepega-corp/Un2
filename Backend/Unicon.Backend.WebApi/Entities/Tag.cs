using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicon.Backend.WebApi.Entities
{
	public class Tag
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public List<DeviceDefinition> RelatedDeviceDefinitions { get; set; } 

	}
}
