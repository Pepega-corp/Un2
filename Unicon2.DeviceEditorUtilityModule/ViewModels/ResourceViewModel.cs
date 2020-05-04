using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResourceViewModel : ViewModelBase, IResourceViewModel
    {
        private bool _isInEditMode;
        private string _resourceName;

        public string StrongName => nameof(ResourceViewModel);

      
        public bool IsInEditMode
        {
            get { return _isInEditMode; }
            set
            {
                _isInEditMode = value;
                RaisePropertyChanged();
            }
        }

        public void StartEditElement()
        {
            IsInEditMode = true;
        }

        public void StopEditElement()
        {
            IsInEditMode = false;
        }

        public string Name
        {
            get { return _resourceName; }
            set
            {
	            _resourceName = value;
                RaisePropertyChanged();
            }
        }

        public INameable RelatedEditorItemViewModel { get; set; }
    }
}
