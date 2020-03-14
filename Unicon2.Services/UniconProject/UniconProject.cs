using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.UniconProject;

namespace Unicon2.Services.UniconProject
{
    [JsonObject(MemberSerialization.OptIn)]
    public class UniconProject : IUniconProject
    {
        private readonly ISerializerService _serializerService;
        private readonly string DefaultProjectName = "DefaultProject";

        public UniconProject(ISerializerService serializerService)
        {
            _serializerService = serializerService;
            Name = DefaultProjectName;
            ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), Name);

            if (ProjectPath != null && !Directory.Exists(Path.Combine(ProjectPath, Name)))
            {
                Directory.CreateDirectory(Path.Combine(ProjectPath, Name));
            }

            _serializerService.SerializeInFile(this, Path.Combine(ProjectPath, Path.ChangeExtension(Name, ".uniproj")));
        }



        public void Dispose()
        {
            ConnectableItems?.Clear();
        }

        [JsonProperty] public List<IConnectable> ConnectableItems { get; set; }

        public bool IsProjectSaved
        {
            get
            {
                if ((ProjectPath != null) && (Name != null) && Name != DefaultProjectName)
                {
                    if (File.Exists(ProjectPath + "\\" + Name + ".uniproj")) return true;
                }

                return false;
            }

        }



        public bool GetIsProjectChanged()
        {
            if ((ProjectPath != null) && (Name != null))
            {
                if (File.Exists(ProjectPath + "\\" + Name + ".uniproj"))
                {
                    try
                    {
                        string existing = _serializerService.SerializeInString(this);
                        string xmlString = File.ReadAllText(ProjectPath + "\\" + Name + ".uniproj");

                        string existing1 = existing.Remove(0, existing.IndexOf("UniconProject"));
                        string xmlString1 = xmlString.Remove(0, xmlString.IndexOf("UniconProject"));
                        //var t = existing1.Length==xmlString1.Length;
                        if (xmlString1 == existing1)
                        {
                            return false;
                        }
                    }
                    catch
                    {
                        return true;
                    }
                }
            }

            return true;
        }

        [JsonProperty] public string Name { get; set; }
        [JsonProperty] public string ProjectPath { get; set; }
        [JsonProperty] public string LayoutString { get; set; }
    }
}