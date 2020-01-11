using System.Runtime.Serialization;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Infrastructure.DeviceInterfaces;
using Unicon2.Infrastructure.FragmentInterfaces.FagmentSettings;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.FileOperations.Model
{
    [DataContract]
    public class FileBrowser : IFileBrowser
    {
        private IBrowserElementFactory _browserElementFactory;
        private string _strongName;

        public FileBrowser(IBrowserElementFactory browserElementFactory)
        {
            this._browserElementFactory = browserElementFactory;
        }

        public void SetDataProvider(IDataProvider dataProvider)
        {
            this._browserElementFactory.SetConnectionProvider(dataProvider);
        }

        public IDeviceDirectory RootDeviceDirectory { get; private set; }

        public async Task LoadRootDirectory()
        {
            this.RootDeviceDirectory =
                this._browserElementFactory.CreateRootDeviceDirectoryBrowserElement();

            await this.RootDeviceDirectory?.Load();
        }

        public string StrongName => FileOperationsKeys.FILE_BROWSER;

        public IFragmentSettings FragmentSettings { get; set; }

        public bool IsInitialized { get; private set; }

        public void InitializeFromContainer(ITypesContainer container)
        {
            this.IsInitialized = true;
            this._browserElementFactory = container.Resolve<IBrowserElementFactory>();
        }
    }
}
