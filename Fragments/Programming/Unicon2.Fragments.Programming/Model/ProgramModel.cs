using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Unicon2.Fragments.Programming.Infrastructure.Keys;
using Unicon2.Fragments.Programming.Infrastructure.Model;
using Unicon2.Fragments.Programming.Infrastructure.Model.Elements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Infrastructure.Services;

namespace Unicon2.Fragments.Programming.Model
{
    [DataContract(Namespace = "ProgramModelNS")]
    public class ProgramModel : IProgramModel
    {
        public const string DEFAULT_FOLDER = "LogicPrograms";
        public const string EXTENSION = ".logprog";
        private readonly ISerializerService _serializerService;
        private IDataProvider _dataProvider;

        public ProgramModel(ISerializerService serializerService)
        {
            _serializerService = serializerService;
            
            this.Schemes = new ISchemeModel[0];
            this.ProjectName = "New Logic Program";
            
            this.ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), DEFAULT_FOLDER);
        }

        [DataMember] public string ProjectName { get; set; }
        [DataMember] public ISchemeModel[] Schemes { get; set; }
        [DataMember] public IConnection[] Connections { get; set; }
        public string StrongName => ProgrammingKeys.PROGRAMMING;
        [DataMember] public IFragmentSettings FragmentSettings { get; set; }
        public string ProjectPath { get; }
        
        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;
        }

        public void SerializeInFile(string elementName, bool isDefaultSaving)
        {
            try
            {
                using (XmlWriter fs = XmlWriter.Create(elementName, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
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
