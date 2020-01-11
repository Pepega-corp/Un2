using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Resources;
using Unicon2.Unity.ViewModels;

namespace Unicon2.DeviceEditorUtilityModule.ViewModels
{
    public class ResourceViewModel : ViewModelBase, IResourceViewModel
    {
        private INameable _resource;
        private string _name;
        private bool _isInEditMode;
        private string _resourceStrongName;

        public string StrongName => nameof(ResourceViewModel);

        public object Model
        {
            get { return this.GetModel(); }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            if (value is INameable)
            {
                this._resource = value as INameable;
                this.Name = (value as INameable).Name;
                if (value is IStronglyNamed)
                {
                    this.ResourceStrongName = (value as IStronglyNamed).StrongName;
                }
            }
        }

        private object GetModel()
        {
            this._resource.Name = this.Name;
            return this._resource;
        }

        public string Name
        {
            get { return this._name; }
            set
            {
                this._name = value;
                RaisePropertyChanged();
            }
        }

        public bool IsInEditMode
        {
            get { return this._isInEditMode; }
            set
            {
                this._isInEditMode = value;
                RaisePropertyChanged();
            }
        }

        public void StartEditElement()
        {
            this.IsInEditMode = true;
        }

        public void StopEditElement()
        {
            this._resource.Name = this.Name;
            this.IsInEditMode = false;
        }

        public string ResourceStrongName
        {
            get { return this._resourceStrongName; }
            set
            {
                this._resourceStrongName = value;
                RaisePropertyChanged();
            }
        }
    }
}
