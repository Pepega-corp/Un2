using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.Elements;

namespace Unicon2.Fragments.Measuring.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringGroup : IMeasuringGroup
    {
        public MeasuringGroup()
        {
            this.MeasuringElements = new List<IMeasuringElement>();
        }
        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public List<IMeasuringElement> MeasuringElements { get; set; }

 
    }
}
