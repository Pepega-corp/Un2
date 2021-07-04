using System.Windows.Controls;
using System.Windows.Input;
using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.Keys;
using Unicon2.Fragments.FileOperations.Infrastructure.Model;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.FragmentInterfaces;
using Unicon2.Infrastructure.Functional;
using Unicon2.Presentation.Infrastructure.DeviceContext;
using Unicon2.Presentation.Infrastructure.ViewModels.FragmentInterfaces.FragmentOptions;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Fragments.FileOperations.ViewModel
{
    public class FileBrowserViewModel : ViewModelBase, IFileBrowserViewModel
    {
        private readonly IBrowserElementViewModelFactory _browserElementViewModelFactory;
        private IFileBrowser _fileBrowser;
        private IDeviceDirectoryViewModel _selectedDirectoryViewModel;

        public IDeviceDirectoryViewModel RootDeviceDirectoryViewModel { get; private set; }

        public IDeviceDirectoryViewModel SelectedDirectoryViewModel
        {
            get { return this._selectedDirectoryViewModel; }
            set
            {
                this._selectedDirectoryViewModel = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SelectDirectoryCommand { get; }

        public ICommand LoadRootCommand { get; }


        public string StrongName => FileOperationsKeys.FILE_BROWSER + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL;

        public string NameForUiKey => FileOperationsKeys.FILE_BROWSER;
        public IFragmentOptionsViewModel FragmentOptionsViewModel { get; set; }

        public DeviceContext DeviceContext { get; set; }

        public FileBrowserViewModel(IBrowserElementViewModelFactory browserElementViewModelFactory)
        {
            this._browserElementViewModelFactory = browserElementViewModelFactory;
            this.LoadRootCommand = new RelayCommand(this.OnLoadRootExecute);
            this.SelectDirectoryCommand = new RelayCommand<object>(this.OnSelectDirectoryExecute);
        }


        private void OnSelectDirectoryExecute(object obj)
        {
            //if ((obj as RoutedPropertyChangedEventArgs<object>)?.NewValue is TreeViewItem)
            //{
            //    this.SelectedDirectoryViewModel =
            //    ((((RoutedPropertyChangedEventArgs<object>)obj).NewValue as TreeViewItem)?.DataContext as
            //        IFileBrowserViewModel)?.RootDeviceDirectoryViewModel;
            //}
            //else
            //{
            //    this.SelectedDirectoryViewModel =
            //        (obj as RoutedPropertyChangedEventArgs<object>)?.NewValue as
            //        IDeviceDirectoryViewModel;
            //}
            if (obj == null) return;

            if (obj is TreeViewItem)
            {
                TreeViewItem item = (TreeViewItem) obj;
                IFileBrowserViewModel vm = item.DataContext as IFileBrowserViewModel;
                this.SelectedDirectoryViewModel = vm?.RootDeviceDirectoryViewModel;
            }
            else
            {
                this.SelectedDirectoryViewModel = obj as IDeviceDirectoryViewModel;
            }
        }

        private async void OnLoadRootExecute()
        {
            if (this._fileBrowser == null) return;
            await this._fileBrowser.LoadRootDirectory();
            this.RootDeviceDirectoryViewModel = this._browserElementViewModelFactory.CreateBrowserElementViewModelBase(this._fileBrowser.RootDeviceDirectory, null) as
                    IDeviceDirectoryViewModel;
            this.RaisePropertyChanged(nameof(this.RootDeviceDirectoryViewModel));
        }

        public Result Initialize(IDeviceFragment deviceFragment)
        {
            if(deviceFragment is IFileBrowser fileBrouser)
            {
                _fileBrowser = fileBrouser;
            }
            return Result.Create(true);
        }
    }
}
