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
            this.SubmitCommand = new RelayCommand(this.OnSubmitExecute);
            this.CloseCommand = new RelayCommand<object>(this.OnCloseExecute);

        }

        private void OnCloseExecute(object obj)
        {
            (obj as Window)?.Close();
        }


        private void OnSubmitExecute()
        {
            this._resource.Name = this.NameKey;
            this._deviceSharedResources.AddResource(this._resource);
            this.IsResourceAdded = true;
            this.RaisePropertyChanged(nameof(this.IsResourceAdded));
        }


        public bool IsResourceAdded { get; private set; }

        public string NameKey
        {
            get { return this._nameKey; }
            set
            {
                this._nameKey = value;
                this.RaisePropertyChanged();
            }
        }

        public ICommand SubmitCommand { get; }

        public ICommand CloseCommand { get; }

        public void Initialize(IDeviceSharedResources deviceSharedResources)
        {
            this._deviceSharedResources = deviceSharedResources;
        }

        public string StrongName => nameof(ResourcesAddingViewModel);

        public object Model
        {
            get { return this._resource; }
            set { this._resource = value as INameable; }
        }
    }
}
