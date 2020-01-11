using Unicon2.Formatting.Infrastructure.ViewModel.InnerMembers;
using Unicon2.Infrastructure.Interfaces;
using Unicon2.Unity.ViewModels;

namespace Unicon2.Formatting.Editor.ViewModels.InnerMembers
{
    public class ArgumentViewModel : ViewModelBase, IArgumentViewModel
    {
        private INameable _resource;
        private string _argumentName;
        private string _resourceNameString;
        private double _testValue;

        public string StrongName => nameof(ArgumentViewModel);

        public object Model
        {
            get { return this._resource; }
            set { this.SetModel(value); }
        }

        private void SetModel(object value)
        {
            this._resource = value as INameable;
            this.ResourceNameString = this._resource.Name;
        }

        public string ArgumentName
        {
            get { return this._argumentName; }
            set
            {
                this._argumentName = value;
                this.RaisePropertyChanged();
            }
        }

        public string ResourceNameString
        {
            get { return this._resourceNameString; }
            set
            {
                this._resourceNameString = value;
                this.RaisePropertyChanged();
            }
        }

        public double TestValue
        {
            get { return this._testValue; }
            set
            {
                this._testValue = value;
                this.RaisePropertyChanged();
            }
        }
    }
}
