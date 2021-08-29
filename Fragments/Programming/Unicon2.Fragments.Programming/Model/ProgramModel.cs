using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProgramModel : IDeviceFragment
    {
        public const string DEFAULT_FOLDER = "LogicPrograms";
        public const string EXTENSION = ".logprog";

        public ProgramModel()
        {
            this.Schemes = new List<SchemeModel>();
            this.Connections = new List<Connection>();
            this.ProjectName = "New Logic Program";
            
            this.ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);

            if (!Directory.Exists(ProjectPath))
            {
                Directory.CreateDirectory(ProjectPath);
            }
        }

        [JsonProperty] public string ProjectName { get; set; }
        [JsonProperty] public List<SchemeModel> Schemes { get; private set; }
        [JsonProperty] public List<Connection> Connections { get; private set; }
        [JsonProperty] public bool EnableFileDriver { get; set; }
        [JsonProperty] public bool WithHeader { get; set; }
        [JsonProperty] public string LogicHeader { get; set; }
        [JsonProperty] public int LogBinSize { get; set; }
        public string StrongName => ProgrammingKeys.PROGRAMMING;
        public IFragmentSettings FragmentSettings { get; set; }
        public string ProjectPath { get; }
    }
}
