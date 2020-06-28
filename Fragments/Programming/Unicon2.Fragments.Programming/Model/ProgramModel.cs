using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProgramModel : IProgramModel
    {
        public const string DEFAULT_FOLDER = "LogicPrograms";
        public const string EXTENSION = ".logprog";

        public ProgramModel()
        {
            this.Schemes = new List<ISchemeModel>();
            this.Connections = new List<IConnection>();
            this.ProjectName = "New Logic Program";
            
            this.ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);

            if (!Directory.Exists(ProjectPath))
            {
                Directory.CreateDirectory(ProjectPath);
            }
        }

        [JsonProperty] public string ProjectName { get; set; }
        [JsonProperty] public List<ISchemeModel> Schemes { get; private set; }
        [JsonProperty] public List<IConnection> Connections { get; private set; }
        public string StrongName => ProgrammingKeys.PROGRAMMING;
        public IFragmentSettings FragmentSettings { get; set; }
        public string ProjectPath { get; }
    }
}
