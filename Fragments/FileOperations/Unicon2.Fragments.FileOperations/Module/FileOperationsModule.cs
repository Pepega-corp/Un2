using Unicon2.Fragments.FileOperations.Factories;
using Unicon2.Fragments.FileOperations.FileOperations;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.FileOperations;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Fragments.FileOperations.Model;
using Unicon2.Fragments.FileOperations.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.ViewModel;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Services;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.FileOperations.Module
{
    public class FileOperationsModule : IUnityModule
    {
        public void Initialize(ITypesContainer container)
        {
            container.Register<IFileBrowser, FileBrowser>();
            container.Register<IFileDriver, FileDriver>();
            container.Register<ICommandSender, CommandSender>();
            container.Register<ICommandStateReader, CommandStateReader>();
            container.Register<IFileDataReader, FileDataReader>();
            container.Register<IFileDataWriter, FileDataWriter>();
            container.Register<IDeviceDirectory, DeviceDirectory>();

            container.Register<IBrowserElementViewModelFactory, BrowserElementViewModelFactory>();
            container.Register<IBrowserElementViewModel, DeviceFileViewModel>(FileOperationsKeys.DEVICE_FILE + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IBrowserElementViewModel, DeviceDirectoryViewModel>(
                FileOperationsKeys.DEVICE_DIRECTORY + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);

            container.Register(typeof(IFragmentViewModel), typeof(FileBrowserViewModel),
                FileOperationsKeys.FILE_BROWSER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            container.Register<IFileBrowserViewModel, FileBrowserViewModel>();

            container.Register<IBrowserElementFactory, BrowserElementFactory>();
        
            container.Resolve<ISerializerService>().AddKnownTypeForSerialization(typeof(FileBrowser));

            container.Resolve<IXamlResourcesService>().AddResourceAsGlobal("Resources/FileOperationsTemplates.xaml", GetType().Assembly);
        }
    }
}
