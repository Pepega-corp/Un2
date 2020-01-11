using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unicon2.Fragments.FileOperations.Editor.Interfaces;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model;
using Unicon2.Infrastructure;

namespace Unicon2.Fragments.FileOperations.Editor.ViewModel
{
   public class FileBrowserEditorViewModel: IFileBrowserEditorViewModel
    {
        private object _model;

        public FileBrowserEditorViewModel(IFileBrowser fileBrowser)
        {
            _model = fileBrowser;
        }


        public string StrongName => FileOperationsKeys.FILE_BROWSER +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        public object Model
        {
            get { return _model; }
            set { _model = value; }
        }

        public string NameForUiKey => FileOperationsKeys.FILE_BROWSER;
    }
}
