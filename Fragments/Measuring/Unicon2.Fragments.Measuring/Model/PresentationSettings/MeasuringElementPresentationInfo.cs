using System;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Model.PresentationSettings
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringElementPresentationInfo : MeasuringPresentationElementBase, IMeasuringElementPresentationInfo
    {
        [JsonProperty] public Guid RelatedMeasuringElementId { get; set; }
    }
}