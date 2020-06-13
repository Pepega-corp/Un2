using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Programming.Model
{
    [JsonObject(MemberSerialization.OptIn)]
    public class ProgramModel : IProgramModel
    {
        public const string DEFAULT_FOLDER = "LogicPrograms";
        public const string EXTENSION = ".logprog";

        public ProgramModel()
        {
            this.Schemes = new ISchemeModel[0];
            this.ProjectName = "New Logic Program";
            
            this.ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);

            if (!Directory.Exists(ProjectPath))
            {
                Directory.CreateDirectory(ProjectPath);
            }
        }

        [JsonProperty] public string ProjectName { get; set; }
        [JsonProperty] public ISchemeModel[] Schemes { get; set; }
        [JsonProperty] public IConnection[] Connections { get; set; }
        public string StrongName => ProgrammingKeys.PROGRAMMING;
        public IFragmentSettings FragmentSettings { get; set; }
        public string ProjectPath { get; }
      
        public void SerializeInFile(string path, bool isDefaultSaving)
        {
            try
            {
                using (XmlWriter fs = XmlWriter.Create(path, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(ProgramModel), this._serializerService.GetTypesForSerialiation());
                    ds.WriteObject(fs, this, this._serializerService.GetNamespacesAttributes());
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeserializeFromFile(string path)
        {
            try
            {
                using (XmlReader fs = XmlReader.Create(path))
                {
                    var ds = new DataContractSerializer(typeof(ProgramModel), this._serializerService.GetTypesForSerialiation());
                    var loadedProgramModel = ((ProgramModel)ds.ReadObject(fs));
                    this.ProjectName = loadedProgramModel.ProjectName;
                    this.Schemes = loadedProgramModel.Schemes;
                    this.Connections = loadedProgramModel.Connections;
                    FragmentSettings = loadedProgramModel.FragmentSettings;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new SerializationException();
            }
        }
    }
}
