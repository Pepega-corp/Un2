using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;

namespace Unicon2.Fragments.FileOperations.Model.BrowserElements
{
   public class DeviceFile:BrowserElementBase,IDeviceFile
    {
        private readonly IFileLoader _fileLoader;

        public DeviceFile(string elementPath,IFileLoader fileLoader,string name,IDeviceDirectory deviceDirectory) : base(elementPath,name,deviceDirectory)
        {
            _fileLoader = fileLoader;
        }

        #region Implementation of IDeviceFile

        public byte[] FileData { get; private set; }

        public void Download()
        {
            this.FileData = _fileLoader.LoadFileData(ElementPath);
           string path= (StaticContainer.Container.Resolve(typeof(IApplicationGlobalCommands)) as IApplicationGlobalCommands)
                .SelectFilePathToSave("Сохранить файл", "", "",Name).GetFirstValue();
            StreamWriter sw=new StreamWriter(path);
            sw.Write(Encoding.UTF8.GetString(this.FileData));
            sw.Close();
        }

        #endregion

        #region Implementation of IDataProviderContaining
        public override string StrongName => FileOperationsKeys.DEVICE_FILE;

        public override async Task Load()
        {
            
        }

        #endregion
    }
}
