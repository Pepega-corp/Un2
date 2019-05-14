using System.Threading.Tasks;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Infrastructure.Interfaces.DataOperations;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements
{
    public interface IDeviceBrowserElement:IStronglyNamed, ILoadable
    {
        IDeviceDirectory ParentDirectory { get; }
        Task<bool> DeleteElementAsync();
        string ElementPath { get; }
        string Name { get; }
    }
}