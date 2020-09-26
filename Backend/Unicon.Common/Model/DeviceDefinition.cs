using System;
using System.Collections.Generic;

namespace Unicon.Common.Model
{
	public class DeviceDefinition
	{
		public int Id { get; set; }
		public string DefinitionString { get; set; }
		public List<string> Tags { get; set; }
		public DateTime LastUpdateDateTime { get; set; }
	}
}
