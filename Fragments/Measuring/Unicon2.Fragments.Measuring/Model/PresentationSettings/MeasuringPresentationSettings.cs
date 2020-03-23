using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Model.PresentationSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringPresentationSettings : IPresentationSettings
    {
        [JsonProperty] public List<IMeasuringPresentationGroup> GroupsOfElements { get; set; }
        [JsonProperty] public List<IMeasuringElementPresentationInfo> Elements { get; set; }
    }
}