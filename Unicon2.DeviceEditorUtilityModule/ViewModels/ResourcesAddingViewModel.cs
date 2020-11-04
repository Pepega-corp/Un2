using System.Windows;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.Infrastructure;
using Unicon2.Infrastructure.Common;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.Factories;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResourcesAddingViewModel : ViewModelBase, IResourcesAddingViewModel
    {

        public ResourcesAddingViewModel()
        {
            SubmitCommand = new RelayCommand(OnSubmitExecute);
            CloseCommand = new RelayCommand<object>(OnCloseExecute);

        }

        private void OnCloseExecute(object obj)
        {
            (obj as Window)?.Close();
        }


        private void OnSubmitExecute()
        {
            if (StaticContainer.Container.Resolve<ISharedResourcesGlobalViewModel>()
                .CheckDeviceSharedResourcesContainsViewModel(ResourceWithName.Name))
            { 
                return;
            }
            IsResourceAdded = true;
            RaisePropertyChanged(nameof(IsResourceAdded));
        }


        public bool IsResourceAdded { get; set; }

        public ICommand SubmitCommand { get; }

        public ICommand CloseCommand { get; }
        public INameable ResourceWithName { get; set; }


        public string StrongName => nameof(ResourcesAddingViewModel);
    }
}
