using System;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model.Elements
{
	[JsonObject(MemberSerialization.OptIn)]
	public abstract class MeasuringElementBase : IMeasuringElement
	{
		private Guid _id;
		public abstract string StrongName { get; }

		[JsonProperty] public string Name { get; set; }

		[JsonProperty]
		public Guid Id
		{
			get
			{
				if (_id == Guid.Empty)
				{
					_id = Guid.NewGuid();
				}
				return _id;
			}
			set => _id = value;
		}
	}
}
