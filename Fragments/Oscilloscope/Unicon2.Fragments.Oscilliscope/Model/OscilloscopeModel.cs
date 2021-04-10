using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unicon2.Fragments.Journals.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Keys;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.CountingTemplate;
using Unicon2.Fragments.Oscilliscope.Infrastructure.Model.OscillogramLoadingParameters;
using Unicon2.Fragments.Oscilliscope.Model.Helpers;
using Unicon2.Infrastructure.Connection;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Progress;
using Unicon2.Infrastructure.Services.UniconProject;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.Oscilliscope.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class OscilloscopeModel : IOscilloscopeModel
    {
        private bool _isInitialized;
        private string _parentDeviceName;

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