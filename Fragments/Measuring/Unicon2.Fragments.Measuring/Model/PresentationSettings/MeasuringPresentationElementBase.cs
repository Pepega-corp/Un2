using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Model.PresentationSettings
{
	[JsonObject(MemberSerialization.OptIn)]
	public class MeasuringPresentationElementBase : IMeasuringPresentationElement
	{
		[JsonProperty] public IPositioningInfo PositioningInfo { get; set; }
	}
}