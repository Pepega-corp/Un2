using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OscilloscopeModel : IOscilloscopeModel
    {   

        public OscilloscopeModel(IUniconJournal uniconJournal,
            ICountingTemplate countingTemplate)
        {
            this.OscilloscopeJournal = uniconJournal;
            this.CountingTemplate = countingTemplate;
        }

        [JsonProperty]
        public IUniconJournal OscilloscopeJournal { get; set; }

        [JsonProperty]
        public IOscillogramLoadingParameters OscillogramLoadingParameters { get; set; }

        [JsonProperty]
        public ICountingTemplate CountingTemplate { get; set; }

        public string StrongName => OscilloscopeKeys.OSCILLOSCOPE;

        [JsonProperty]
        public IFragmentSettings FragmentSettings { get; set; }
        

    }
}