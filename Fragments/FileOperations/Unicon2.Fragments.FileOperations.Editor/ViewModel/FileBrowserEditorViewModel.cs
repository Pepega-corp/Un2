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



        #region Implementation of IStronglyNamed

        public string StrongName => FileOperationsKeys.FILE_BROWSER +
                                    ApplicationGlobalNames.CommonInjectionStrings.EDITOR_VIEWMODEL;

        #endregion

        #region Implementation of IViewModel

        public object Model
        {
            get { return _model; }
            set { _model = value; }
        }

        #endregion

        #region Implementation of IFragmentEditorViewModel

        public string NameForUiKey => FileOperationsKeys.FILE_BROWSER;

        #endregion
    }
}
