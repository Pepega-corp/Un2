using Unicon2.Infrastructure.Interfaces;
using Unicon2.Presentation.Infrastructure.ViewModels.Values;
using Unicon2.Presentation.Values.Base;

namespace Unicon2.Presentation.Values
{
    public class BoolValueViewModel : FormattableValueViewModelBase, IBoolValueViewModel, IStronglyNamedDynamic
    {
        private bool _boolValueProperty;

        private string _strongName=nameof(BoolValueViewModel);

        public override string StrongName => _strongName;

        public override string AsString()
        {
            return BoolValueProperty.ToString();
        }

        public bool BoolValueProperty
        {
            get { return _boolValueProperty; }
            set
            {
                _boolValueProperty = value;
                RaisePropertyChanged();
            }
        }

        public void SetBoolValueProperty(bool value)
        {
            BoolValueProperty = value;
        }

        public void SetStrongName(string name)
        {
            _strongName = name;
        }
    }
}