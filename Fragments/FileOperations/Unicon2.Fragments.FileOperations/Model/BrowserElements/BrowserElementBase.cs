using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Infrastructure.DeviceInterfaces;

namespace Unicon2.Fragments.FileOperations.Model.BrowserElements
{
    public abstract class BrowserElementBase : IDeviceBrowserElement
    {
        protected IDataProvider _dataProvider;
        private string _strongName;
        private string _name;

        protected BrowserElementBase(string elementPath, string name, IDeviceDirectory parentDirectory)
        {
            this.ElementPath = elementPath;
            this.Name = name;
            this.ParentDirectory = parentDirectory;
        }

        public IDeviceDirectory ParentDirectory { get; }

        public async Task<bool> DeleteElementAsync()
        {
            return await this.ParentDirectory.RemoveChildElementAsync(this);
        }

        public string ElementPath { get; }

        public string Name { get; }

        public virtual void SetDataProvider(IDataProvider dataProvider)
        {
            this._dataProvider = dataProvider;

        }

        public abstract Task Load();

        public abstract string StrongName { get; }
    }
}
