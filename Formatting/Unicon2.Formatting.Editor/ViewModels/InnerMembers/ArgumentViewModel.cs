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
            get { return _resource; }
            set { SetModel(value); }
        }

        private void SetModel(object value)
        {
            _resource = value as INameable;
            ResourceNameString = _resource.Name;
        }

        public string ArgumentName
        {
            get { return _argumentName; }
            set
            {
                _argumentName = value;
                RaisePropertyChanged();
            }
        }

        public string ResourceNameString
        {
            get { return _resourceNameString; }
            set
            {
                _resourceNameString = value;
                RaisePropertyChanged();
            }
        }

        public double TestValue
        {
            get { return _testValue; }
            set
            {
                _testValue = value;
                RaisePropertyChanged();
            }
        }
    }
}
