using Unicon2.Fragments.FileOperations.Infrastructure.Factories;
using Unicon2.Fragments.FileOperations.Infrastructure.Model.BrowserElements;
using Unicon2.Fragments.FileOperations.Infrastructure.ViewModel.BrowserElements;
using Unicon2.Infrastructure;
using Unicon2.Unity.Interfaces;

namespace Unicon2.Fragments.FileOperations.Factories
{
    public class BrowserElementViewModelFactory : IBrowserElementViewModelFactory
    {
        private readonly ITypesContainer _container;

        public BrowserElementViewModelFactory(ITypesContainer container)
        {
            this._container = container;
        }

        public IBrowserElementViewModel CreateBrowserElementViewModelBase(IDeviceBrowserElement deviceBrowserElement, IDeviceDirectoryViewModel parentDeviceDirectoryViewModel)
        {
            IBrowserElementViewModel browserElementViewModel =
                this._container.Resolve<IBrowserElementViewModel>(
                    deviceBrowserElement.StrongName + ApplicationGlobalNames.CommonInjectionStrings.VIEW_MODEL);
            browserElementViewModel.Model = deviceBrowserElement;
            browserElementViewModel.ParentDeviceDirectoryViewModel = parentDeviceDirectoryViewModel;
            return browserElementViewModel;
        }
    }
}
