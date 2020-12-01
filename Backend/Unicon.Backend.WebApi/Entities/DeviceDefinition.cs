using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Unicon.Backend.WebApi.Entities
{
	public class DeviceDefinition
	{
		public int Id { get; set; }
		public string DefinitionString { get; set; }
		public List<Tag> Tags { get; set; }
		public DateTime LastUpdateDateTime { get; set; }
	}
}
