using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using Unicon2.Infrastructure.Extensions;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Services;
using Unicon2.Infrastructure.Services.UniconProject;

namespace Unicon2.Services.UniconProject
{
    [DataContract]
    public class UniconProject : IUniconProject
    {
        private readonly ISerializerService _serializerService;
        private readonly string TempProjectName = "TempProject";

        public UniconProject(ISerializerService serializerService)
        {
            this._serializerService = serializerService;
            this.Name = this.TempProjectName;
            this.ProjectPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), this.Name);

            if (this.ProjectPath != null && !Directory.Exists(Path.Combine(this.ProjectPath, this.Name)))
            {
                Directory.CreateDirectory(Path.Combine(this.ProjectPath, this.Name));
            }
            this.SerializeInFile(Path.Combine(this.ProjectPath, Path.ChangeExtension(this.Name, ".uniproj")), false);
        }


        #region Implementation of ISerializableInFile

        public void SerializeInFile(string elementName, bool isDefaultSaving)
        {
            try
            {
                using (XmlWriter fs = XmlWriter.Create(elementName, new XmlWriterSettings { Indent = true, Encoding = Encoding.UTF8 }))
                {
                    DataContractSerializer ds = new DataContractSerializer(typeof(UniconProject), this._serializerService.GetTypesForSerialiation());

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
                    DataContractSerializer ds = new DataContractSerializer(typeof(UniconProject), this._serializerService.GetTypesForSerialiation());
                    UniconProject project = ((UniconProject)ds.ReadObject(fs));
                    this.ConnectableItems = project.ConnectableItems;
                    this.LayoutString = project.LayoutString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new SerializationException();
            }
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            this.ConnectableItems?.Clear();
        }

        #endregion

        #region Implementation of IUniconProject
        [DataMember]
        public List<IConnectable> ConnectableItems { get; set; }

        public bool IsProjectSaved
        {
            get
            {
                if ((this.ProjectPath != null) && (this.Name != null) && this.Name != this.TempProjectName)
                {
                    if (File.Exists(this.ProjectPath + "\\" + this.Name + ".uniproj")) return true;
                }
                return false;
            }

        }



        public bool GetIsProjectChanged()
        {
            if ((this.ProjectPath != null) && (this.Name != null))
            {
                if (File.Exists(this.ProjectPath + "\\" + this.Name + ".uniproj"))
                {
                    try
                    {
                        StringBuilder stringBuilder = new StringBuilder();

                        using (XmlWriter fs = XmlWriter.Create(stringBuilder, new XmlWriterSettings() { Indent = true, Encoding = Encoding.UTF8 }))
                        {
                            DataContractSerializer ds = new DataContractSerializer(typeof(UniconProject),
                                this._serializerService.GetTypesForSerialiation());

                            ds.WriteObject(fs, this, this._serializerService.GetNamespacesAttributes());
                        }
                        string existing = stringBuilder.ToString();
                        string xmlString = System.IO.File.ReadAllText(this.ProjectPath + "\\" + this.Name + ".uniproj");

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

        public string Name { get; set; }
        public string ProjectPath { get; set; }

        public string LayoutString { get; set; }

        #endregion
    }
}
