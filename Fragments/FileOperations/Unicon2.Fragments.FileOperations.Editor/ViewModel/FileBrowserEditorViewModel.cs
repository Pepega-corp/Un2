using System;
using Unicon2.Fragments.FileOperations.Editor.Interfaces;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;

namespace Unicon2.Fragments.FileOperations.Editor.ViewModel
{
   public class FileBrowserEditorViewModel: IFileBrowserEditorViewModel
    {
        public string StrongName => FileOperationsKeys.FILE_BROWSER +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public IFileBrowser Model { get; private set; }

        public string NameForUiKey => FileOperationsKeys.FILE_BROWSER;

        public IDeviceFragment BuildDeviceFragment() => throw new NotImplementedException();
        public Result Initialize(IDeviceFragment deviceFragment)
        {
            Model = deviceFragment as IFileBrowser;
            return Result.Create(true);
        }
    }
}
