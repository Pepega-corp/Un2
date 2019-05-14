using System.Collections.Generic;
using System.Threading.Tasks;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements
{
    public interface IDeviceDirectory:IDeviceBrowserElement
    {
        List<IDeviceBrowserElement> BrowserElementsInDirectory { get; }
        Task<bool> RemoveChildElementAsync(IDeviceBrowserElement browserElement);
        Task<bool> CreateNewChildDirectoryAsync(string directoryName);
        Task<string> AddNewChildFileAsync(byte[] file,string name,string extension);
    }
}