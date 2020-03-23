using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Model.PresentationSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringPresentationElementBase : IMeasuringPresentationElement
    {
        [JsonProperty] public int OffsetLeft { get; set; }
        [JsonProperty] public int OffsetTop { get; set; }
        [JsonProperty] public int SizeWidth { get; set; }
        [JsonProperty] public int SizeHeight { get; set; }
    }
}