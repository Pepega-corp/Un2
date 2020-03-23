using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;

namespace Unicon2.Fragments.Measuring.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringGroup : IMeasuringGroup
    {
        public MeasuringGroup()
        {
            MeasuringElements = new List<IMeasuringElement>();
        }

        [JsonProperty] public string Name { get; set; }

        [JsonProperty] public List<IMeasuringElement> MeasuringElements { get; set; }
        [JsonProperty] public IPresentationSettings PresentationSettings { get; set; }
    }
}