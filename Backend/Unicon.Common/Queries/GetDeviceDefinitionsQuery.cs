using System.Collections.Generic;
using Unicon.Common.Interfaces;
using Unicon.Common.Model;

namespace Unicon.Common.Queries
{
	public class GetDeviceDefinitionsQuery : Query<List<DeviceDefinition>>
	{
		public GetDeviceDefinitionsQuery(string tag = null, List<string> ids = null)
		{
			Tag = tag;
			Ids = ids;
		}

		public string Tag { get; }
		public List<string> Ids { get; }

	}
}