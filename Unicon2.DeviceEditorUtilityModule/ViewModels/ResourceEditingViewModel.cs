using System.Windows;
using System.Windows.Input;
using Unicon2.DeviceEditorUtilityModule.Interfaces.DeviceSharedResources;
using Unicon2.Infrastructure.Interfaces.EditOperations;
using Unicon2.Infrastructure.ViewModel;
using Unicon2.Unity.Commands;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResourceEditingViewModel : ViewModelBase, IResourceEditingViewModel
    {
        private IViewModel _resourceEditorViewModel;

        public ResourceEditingViewModel()
        {
            this.CloseCommand = new RelayCommand<object>(OnCloseExecute);
        }

        private void OnCloseExecute(object obj)
        {
            if (this._resourceEditorViewModel is IEditable)
            {
                (this._resourceEditorViewModel as IEditable).StopEditElement();
            }
            if (obj is Window)
            {
                (obj as Window).Close();
            }
        }


        public IViewModel ResourceEditorViewModel
        {
            get { return this._resourceEditorViewModel; }
            set
            {
                this._resourceEditorViewModel = value;
                RaisePropertyChanged();
            }
        }

        public ICommand CloseCommand { get; }
    }
}