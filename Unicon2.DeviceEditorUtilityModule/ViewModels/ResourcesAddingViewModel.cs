using System.Windows;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.Infrastructure.DeviceInterfaces.SharedResources;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResourcesAddingViewModel : ViewModelBase, IResourcesAddingViewModel
    {
        private string _nameKey;
        private INameable _resource;
        private IDeviceSharedResources _deviceSharedResources;

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
            _resource.Name = NameKey;
            _deviceSharedResources.AddResource(_resource);
            IsResourceAdded = true;
            RaisePropertyChanged(nameof(IsResourceAdded));
        }


        public bool IsResourceAdded { get; private set; }

        public string NameKey
        {
            get { return _nameKey; }
            set
            {
                _nameKey = value;
                RaisePropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; }

        public ICommand CloseCommand { get; }

        public void Initialize(IDeviceSharedResources deviceSharedResources)
        {
            _deviceSharedResources = deviceSharedResources;
        }

        public string StrongName => nameof(ResourcesAddingViewModel);

        public object Model
        {
            get { return _resource; }
            set { _resource = value as INameable; }
        }
    }
}
