using Unicon2.Infrastructure.Interfaces;

namespace Unicon2.Fragments.FileOperations.Infrastructure.Model.Loaders
{
    public interface IFileLoader
    {
        byte[] LoadFileData(string filePath);
        void SetDataProvider(object dataprovider);

    }
}