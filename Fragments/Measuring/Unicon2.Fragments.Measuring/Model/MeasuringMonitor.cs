using System.Collections.Generic;
using Newtonsoft.Json;
using Unicon2.Fragments.Measuring.Infrastructure.Keys;
using Unicon2.Fragments.Measuring.Infrastructure.Model;
using Unicon2.Fragments.Measuring.Infrastructure.Model.PresentationSettings;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Measuring.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MeasuringMonitor : IMeasuringMonitor
    {
        public MeasuringMonitor()
        {
            MeasuringGroups = new List<IMeasuringGroup>();
        }

        public string StrongName => MeasuringKeys.MEASURING_MONITOR;

        [JsonProperty]
        public IFragmentSettings FragmentSettings { get; set; }

        [JsonProperty]
        public List<IMeasuringGroup> MeasuringGroups { get; set; }
        [JsonProperty]
        public IPresentationSettings PresentationSettings { get; set; }
    }
}
